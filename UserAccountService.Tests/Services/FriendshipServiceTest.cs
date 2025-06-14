using System;
using System.Collections.Generic;
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

public class FriendshipServiceTest : IDisposable
{
    private readonly UserAccountDbContext _context;
    private readonly FriendshipService _friendshipService;
    private readonly Mock<IHubClients> _mockClients;
    private readonly Mock<IClientProxy> _mockClientProxy;

    private readonly User _user1;
    private readonly User _user2;

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
        _mockClients.Setup(c => c.Groups(It.IsAny<IReadOnlyList<string>>())).Returns(_mockClientProxy.Object);

        _friendshipService = new FriendshipService(_context, mockLogger.Object, mockHubContext.Object);

        _user1 = new User
        { Id = Guid.NewGuid(), Name = "User One", Tag = "one#1111", PasswordHash = "hash", Email = "on@on.on" };
        _user2 = new User
        { Id = Guid.NewGuid(), Name = "User Two", Tag = "two#2222", PasswordHash = "hash", Email = "tw@tw.tw" };
        var user3 = new User
        { Id = Guid.NewGuid(), Name = "User Three", Tag = "three#3333", PasswordHash = "hash", Email = "tr@tr.tr" };

        _context.Users.AddRange(_user1, _user2, user3);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

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

    [Fact]
    public async Task SendFriendRequestAsync_ToNonExistentUser_ReturnsError()
    {
        var result = await _friendshipService.SendFriendRequestAsync(_user1.Id, Guid.NewGuid());

        Assert.False(result.Success);
        Assert.Equal("Recipient user not found.", result.Message);
    }

    [Fact]
    public async Task SendFriendRequestAsync_ToSelf_ReturnsError()
    {
        var result = await _friendshipService.SendFriendRequestAsync(_user1.Id, _user1.Id);

        Assert.False(result.Success);
        Assert.Equal("Cannot send a friend request to yourself.", result.Message);
    }

    [Fact]
    public async Task AcceptFriendRequestAsync_WithValidRequest_AcceptsAndNotifiesBothUsers()
    {
        var request = new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = _user1.Id,
            AddresseeId = _user2.Id,
            Status = FriendshipStatus.Pending
        };
        await _context.Friendships.AddAsync(request);
        await _context.SaveChangesAsync();

        var result = await _friendshipService.AcceptFriendRequestAsync(request.Id, _user2.Id);

        Assert.True(result.Success);
        Assert.Equal(FriendshipStatus.Accepted, result.Status);
        var friendship = await _context.Friendships.FindAsync(request.Id);
        Assert.NotNull(friendship);
        Assert.Equal(FriendshipStatus.Accepted, friendship.Status);

        _mockClients.Verify(clients => clients.Group(_user1.Id.ToString()), Times.Once);
        _mockClients.Verify(clients => clients.Group(_user2.Id.ToString()), Times.Once);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "FriendRequestProcessed",
                It.Is<object[]>(o => o.Length == 0),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task AcceptFriendRequestAsync_WithNonExistentRequest_ReturnsError()
    {
        var result = await _friendshipService.AcceptFriendRequestAsync(Guid.NewGuid(), _user2.Id);

        Assert.False(result.Success);
        Assert.Equal("Friend request not found.", result.Message);
    }

    [Fact]
    public async Task DeclineFriendRequestAsync_ByAddressee_RemovesRequestAndNotifies()
    {
        var request = new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = _user1.Id,
            AddresseeId = _user2.Id,
            Status = FriendshipStatus.Pending
        };
        await _context.Friendships.AddAsync(request);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = await _friendshipService.DeclineFriendRequestAsync(request.Id, _user2.Id);

        Assert.True(result.Success);
        Assert.Equal(FriendshipStatus.Declined, result.Status);
        var friendship = await _context.Friendships.FindAsync(request.Id);
        Assert.Null(friendship);

        _mockClients.Verify(clients => clients.Group(_user1.Id.ToString()), Times.Once);
        _mockClients.Verify(clients => clients.Group(_user2.Id.ToString()), Times.Once);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "FriendRequestProcessed",
                It.Is<object[]>(o => o.Length == 0),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task RemoveFriendAsync_WithExistingFriendship_RemovesFriendAndNotifies()
    {
        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = _user1.Id,
            AddresseeId = _user2.Id,
            Status = FriendshipStatus.Accepted,
            RespondedAt = DateTime.UtcNow
        };
        await _context.Friendships.AddAsync(friendship);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = await _friendshipService.RemoveFriendAsync(_user1.Id, _user2.Id);

        Assert.True(result.Success);
        var deletedFriendship = await _context.Friendships.FindAsync(friendship.Id);
        Assert.Null(deletedFriendship);

        _mockClients.Verify(clients => clients.Group(_user2.Id.ToString()), Times.Once);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "FriendshipRemoved",
                It.Is<object[]>(o => o.Length == 0),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}