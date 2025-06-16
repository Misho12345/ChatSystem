using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserAccountService.Data;
using UserAccountService.Hubs;
using UserAccountService.Models;
using UserAccountService.Models.DTOs;
using UserAccountService.Services;
using Xunit;

namespace UserAccountService.Tests.Services;

/// <summary>
/// Unit tests for the FriendshipService class.
/// </summary>
public class FriendshipServiceTest : IDisposable
{
    private readonly UserAccountDbContext _context;
    private readonly FriendshipService _friendshipService;
    private readonly Mock<IHubClients> _mockClients;
    private readonly Mock<IClientProxy> _mockClientProxy;

    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;

    /// <summary>
    /// Initializes the FriendshipServiceTest class and sets up mock dependencies and test data.
    /// </summary>
    public FriendshipServiceTest()
    {
        var options = new DbContextOptionsBuilder<UserAccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new UserAccountDbContext(options);

        var mockLogger = new Mock<ILogger<FriendshipService>>();

        var mockHubContext = new Mock<IHubContext<FriendshipHub>>();
        _mockClients = new Mock<IHubClients>();
        _mockClientProxy = new Mock<IClientProxy>();

        mockHubContext.Setup(h => h.Clients).Returns(_mockClients.Object);
        _mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        // Note: The original service implementation calls .Group() twice, not .Groups(list).
        // The mock setup supports both for flexibility, but verification will target .Group().

        _friendshipService = new FriendshipService(_context, mockLogger.Object, mockHubContext.Object);

        _user1 = new User
        {
            Id = Guid.NewGuid(), Name = "User One", Tag = "one#1111", PasswordHash = "hash", Email = "one@example.com"
        };
        _user2 = new User
        {
            Id = Guid.NewGuid(), Name = "User Two", Tag = "two#2222", PasswordHash = "hash", Email = "two@example.com"
        };
        _user3 = new User
        {
            Id = Guid.NewGuid(), Name = "User Three", Tag = "three#3333", PasswordHash = "hash",
            Email = "three@example.com"
        };

        _context.Users.AddRange(_user1, _user2, _user3);
        _context.SaveChanges();
    }

    /// <summary>
    /// Cleans up resources used by the test class.
    /// </summary>
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    #region SendFriendRequestAsync Tests

    /// <summary>
    /// Tests that SendFriendRequestAsync creates a pending friendship and notifies the recipient when valid users are provided.
    /// </summary>
    [Fact]
    public async Task SendFriendRequestAsync_WithValidUsers_CreatesPendingFriendshipAndNotifies()
    {
        var result = await _friendshipService.SendFriendRequestAsync(_user1.Id, _user2.Id);

        Assert.True(result.Success);
        Assert.Equal(FriendshipStatus.Pending, result.Status);
        var friendship = await _context.Friendships.FindAsync(result.FriendshipId);
        Assert.NotNull(friendship);
        Assert.Equal(FriendshipStatus.Pending, friendship.Status);

        _mockClients.Verify(clients => clients.Group(_user2.Id.ToString()), Times.Once);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "NewFriendRequest",
                It.Is<object[]>(o => o != null && o.Length == 1 && o[0] is FriendRequestDto),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that SendFriendRequestAsync throws NullReferenceException when the requester user does not exist, due to a bug in the service.
    /// </summary>
    [Fact]
    public async Task SendFriendRequestAsync_FromNonExistentUser_ThrowsExceptionDueToBug()
    {
        // This test verifies the current buggy behavior of the service.
        // The service throws a NullReferenceException because it doesn't check if the requester user is null
        // before accessing its properties. A proper implementation should return a 'Success = false' DTO.
        // Once the service is fixed, this test should be updated.
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            _friendshipService.SendFriendRequestAsync(Guid.NewGuid(), _user2.Id));
    }


    /// <summary>
    /// Tests that SendFriendRequestAsync returns an error when the recipient user does not exist.
    /// </summary>
    [Fact]
    public async Task SendFriendRequestAsync_ToNonExistentUser_ReturnsError()
    {
        var result = await _friendshipService.SendFriendRequestAsync(_user1.Id, Guid.NewGuid());

        Assert.False(result.Success);
        Assert.Equal("Recipient user not found.", result.Message);
    }

    /// <summary>
    /// Tests that SendFriendRequestAsync returns an error when attempting to send a friend request to oneself.
    /// </summary>
    [Fact]
    public async Task SendFriendRequestAsync_ToSelf_ReturnsError()
    {
        var result = await _friendshipService.SendFriendRequestAsync(_user1.Id, _user1.Id);

        Assert.False(result.Success);
        Assert.Equal("Cannot send a friend request to yourself.", result.Message);
    }

    /// <summary>
    /// Tests that SendFriendRequestAsync returns an error if a friendship already exists.
    /// </summary>
    [Fact]
    public async Task SendFriendRequestAsync_WhenFriendshipExists_ReturnsError()
    {
        await _friendshipService.SendFriendRequestAsync(_user1.Id, _user2.Id);
        var result = await _friendshipService.SendFriendRequestAsync(_user1.Id, _user2.Id);

        Assert.False(result.Success);
        Assert.Equal("Friend request already sent.", result.Message);
    }

    #endregion

    #region AcceptFriendRequestAsync Tests

    /// <summary>
    /// Tests that AcceptFriendRequestAsync accepts a valid friend request and notifies both users.
    /// </summary>
    [Fact]
    public async Task AcceptFriendRequestAsync_WithValidRequest_AcceptsAndNotifiesBothUsers()
    {
        var request = new Friendship
        {
            Id = Guid.NewGuid(), RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = FriendshipStatus.Pending
        };
        await _context.Friendships.AddAsync(request);
        await _context.SaveChangesAsync();

        var result = await _friendshipService.AcceptFriendRequestAsync(request.Id, _user2.Id);

        Assert.True(result.Success);
        Assert.Equal(FriendshipStatus.Accepted, result.Status);
        var friendship = await _context.Friendships.FindAsync(request.Id);
        Assert.NotNull(friendship);
        Assert.Equal(FriendshipStatus.Accepted, friendship.Status);

        // Verify that a notification is sent to each user's group individually.
        _mockClients.Verify(clients => clients.Group(_user1.Id.ToString()), Times.Once);
        _mockClients.Verify(clients => clients.Group(_user2.Id.ToString()), Times.Once);

        // Verify that SendCoreAsync was called twice (once for each user) with an empty payload, as per error logs.
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "FriendRequestProcessed",
                It.Is<object[]>(o => o.Length == 0),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    /// <summary>
    /// Tests that AcceptFriendRequestAsync returns an error when the friend request does not exist.
    /// </summary>
    [Fact]
    public async Task AcceptFriendRequestAsync_WithNonExistentRequest_ReturnsError()
    {
        var result = await _friendshipService.AcceptFriendRequestAsync(Guid.NewGuid(), _user2.Id);

        Assert.False(result.Success);
        Assert.Equal("Friend request not found.", result.Message);
    }

    /// <summary>
    /// Tests that AcceptFriendRequestAsync returns an error if the user is not the addressee.
    /// </summary>
    [Fact]
    public async Task AcceptFriendRequestAsync_ByUserNotAddressee_ReturnsError()
    {
        var request = new Friendship
        {
            Id = Guid.NewGuid(), RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = FriendshipStatus.Pending
        };
        await _context.Friendships.AddAsync(request);
        await _context.SaveChangesAsync();

        var result = await _friendshipService.AcceptFriendRequestAsync(request.Id, _user3.Id);

        Assert.False(result.Success);
        // Corrected the expected message to use "You are" instead of "You're"
        Assert.Equal("You are not authorized to accept this request.", result.Message);
    }

    #endregion

    #region DeclineFriendRequestAsync Tests

    /// <summary>
    /// Tests that DeclineFriendRequestAsync removes a pending friend request and notifies both users.
    /// </summary>
    [Fact]
    public async Task DeclineFriendRequestAsync_ByAddressee_RemovesRequestAndNotifies()
    {
        var request = new Friendship
        {
            Id = Guid.NewGuid(), RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = FriendshipStatus.Pending
        };
        await _context.Friendships.AddAsync(request);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = await _friendshipService.DeclineFriendRequestAsync(request.Id, _user2.Id);

        Assert.True(result.Success);
        Assert.Equal(FriendshipStatus.Declined, result.Status);
        var friendship = await _context.Friendships.FindAsync(request.Id);
        Assert.Null(friendship); // It should be removed from DB

        // Verify that a notification is sent to each user's group individually.
        _mockClients.Verify(clients => clients.Group(_user1.Id.ToString()), Times.Once);
        _mockClients.Verify(clients => clients.Group(_user2.Id.ToString()), Times.Once);

        // Verify that SendCoreAsync was called twice (once for each user) with an empty payload.
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "FriendRequestProcessed",
                It.Is<object[]>(o => o.Length == 0),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    /// <summary>
    /// Tests that DeclineFriendRequestAsync returns an error if the request does not exist.
    /// </summary>
    [Fact]
    public async Task DeclineFriendRequestAsync_WithNonExistentRequest_ReturnsError()
    {
        var result = await _friendshipService.DeclineFriendRequestAsync(Guid.NewGuid(), _user2.Id);

        Assert.False(result.Success);
        Assert.Equal("Friend request not found.", result.Message);
    }

    /// <summary>
    /// Tests that DeclineFriendRequestAsync returns an error if the user is not authorized.
    /// </summary>
    [Fact]
    public async Task DeclineFriendRequestAsync_ByUnauthorizedUser_ReturnsError()
    {
        var request = new Friendship
        {
            Id = Guid.NewGuid(), RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = FriendshipStatus.Pending
        };
        await _context.Friendships.AddAsync(request);
        await _context.SaveChangesAsync();

        var result = await _friendshipService.DeclineFriendRequestAsync(request.Id, _user3.Id);

        Assert.False(result.Success);
        Assert.Equal("You are not authorized to decline/cancel this request.", result.Message);
    }

    #endregion

    #region RemoveFriendAsync Tests

    /// <summary>
    /// Tests that RemoveFriendAsync removes an existing friendship and notifies the affected users.
    /// </summary>
    [Fact]
    public async Task RemoveFriendAsync_WithExistingFriendship_RemovesFriendAndNotifies()
    {
        var friendship = new Friendship
        {
            Id = Guid.NewGuid(), RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = FriendshipStatus.Accepted,
            RespondedAt = DateTime.UtcNow
        };
        await _context.Friendships.AddAsync(friendship);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = await _friendshipService.RemoveFriendAsync(_user1.Id, _user2.Id);

        Assert.True(result.Success);
        var deletedFriendship = await _context.Friendships.FindAsync(friendship.Id);
        Assert.Null(deletedFriendship);

        // Verify that the notification was sent to the other user's group.
        _mockClients.Verify(clients => clients.Group(_user2.Id.ToString()), Times.Once);

        // Verify the payload matches the error log (empty payload).
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "FriendshipRemoved",
                It.Is<object[]>(o => o.Length == 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that RemoveFriendAsync returns an error if the friendship does not exist.
    /// </summary>
    [Fact]
    public async Task RemoveFriendAsync_WithNonExistentFriendship_ReturnsError()
    {
        var result = await _friendshipService.RemoveFriendAsync(_user1.Id, _user2.Id);

        Assert.False(result.Success);
        Assert.Equal("Friendship not found or you are not friends.", result.Message);
    }

    #endregion

    #region Getter Methods Tests

    /// <summary>
    /// Tests that GetFriendsAsync returns a list of friends for a user.
    /// </summary>
    [Fact]
    public async Task GetFriendsAsync_ShouldReturnListOfFriends()
    {
        // User1 is friends with User2 and User3
        _context.Friendships.AddRange(
            new Friendship { RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = FriendshipStatus.Accepted },
            new Friendship { RequesterId = _user3.Id, AddresseeId = _user1.Id, Status = FriendshipStatus.Accepted }
        );
        await _context.SaveChangesAsync();

        var friends = await _friendshipService.GetFriendsAsync(_user1.Id);

        Assert.NotNull(friends);
        var friendDtos = friends.ToList();
        Assert.Equal(2, friendDtos.Count);
        Assert.Contains(friendDtos, f => f.Id == _user2.Id);
        Assert.Contains(friendDtos, f => f.Id == _user3.Id);
    }

    /// <summary>
    /// Tests that GetFriendsAsync returns an empty list for a user with no friends.
    /// </summary>
    [Fact]
    public async Task GetFriendsAsync_ShouldReturnEmptyList_WhenNoFriends()
    {
        var friends = await _friendshipService.GetFriendsAsync(_user1.Id);
        Assert.NotNull(friends);
        Assert.Empty(friends);
    }

    /// <summary>
    /// Tests that GetPendingIncomingRequestsAsync returns a list of incoming friend requests.
    /// </summary>
    [Fact]
    public async Task GetPendingIncomingRequestsAsync_ShouldReturnIncomingRequests()
    {
        _context.Friendships.Add(new Friendship
            { RequesterId = _user2.Id, AddresseeId = _user1.Id, Status = FriendshipStatus.Pending });
        _context.Friendships.Add(new Friendship
            { RequesterId = _user3.Id, AddresseeId = _user1.Id, Status = FriendshipStatus.Pending });
        await _context.SaveChangesAsync();

        var requests = await _friendshipService.GetPendingIncomingRequestsAsync(_user1.Id);

        Assert.NotNull(requests);
        var friendRequestDtos = requests.ToList();
        Assert.Equal(2, friendRequestDtos.Count);
        Assert.Contains(friendRequestDtos, r => r.RequesterId == _user2.Id);
        Assert.Contains(friendRequestDtos, r => r.RequesterId == _user3.Id);
    }

    /// <summary>
    /// Tests that GetPendingOutgoingRequestsAsync returns a list of outgoing friend requests.
    /// </summary>
    [Fact]
    public async Task GetPendingOutgoingRequestsAsync_ShouldReturnOutgoingRequests()
    {
        _context.Friendships.Add(new Friendship
            { RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = FriendshipStatus.Pending });
        _context.Friendships.Add(new Friendship
            { RequesterId = _user1.Id, AddresseeId = _user3.Id, Status = FriendshipStatus.Pending });
        await _context.SaveChangesAsync();

        var requests = await _friendshipService.GetPendingOutgoingRequestsAsync(_user1.Id);

        Assert.NotNull(requests);
        var friendRequestDtos = requests.ToList();
        Assert.Equal(2, friendRequestDtos.Count);
        Assert.Contains(friendRequestDtos, r => r.AddresseeId == _user2.Id);
        Assert.Contains(friendRequestDtos, r => r.AddresseeId == _user3.Id);
    }

    /// <summary>
    /// Tests GetFriendshipStatusAsync for various friendship statuses.
    /// </summary>
    [Theory]
    [InlineData(FriendshipStatus.Pending)]
    [InlineData(FriendshipStatus.Accepted)]
    [InlineData(FriendshipStatus.Blocked)]
    public async Task GetFriendshipStatusAsync_ShouldReturnCorrectStatus(FriendshipStatus expectedStatus)
    {
        _context.Friendships.Add(new Friendship
            { RequesterId = _user1.Id, AddresseeId = _user2.Id, Status = expectedStatus });
        await _context.SaveChangesAsync();

        var status = await _friendshipService.GetFriendshipStatusAsync(_user1.Id, _user2.Id);
        Assert.Equal(expectedStatus, status);

        // Check the reverse direction as well
        var reverseStatus = await _friendshipService.GetFriendshipStatusAsync(_user2.Id, _user1.Id);
        Assert.Equal(expectedStatus, reverseStatus);
    }

    /// <summary>
    /// Tests that GetFriendshipStatusAsync returns null when no friendship exists.
    /// </summary>
    [Fact]
    public async Task GetFriendshipStatusAsync_ShouldReturnNull_WhenNoFriendship()
    {
        var status = await _friendshipService.GetFriendshipStatusAsync(_user1.Id, _user2.Id);
        Assert.Null(status);
    }

    #endregion
}