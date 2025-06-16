using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UserAccountService.Controllers;
using UserAccountService.Models;
using UserAccountService.Models.DTOs;
using UserAccountService.Services;
using Xunit;
using Microsoft.Extensions.Logging;

namespace UserAccountService.Tests.Controllers;

/// <summary>
/// Unit tests for the FriendsController class.
/// </summary>
public class FriendsControllerTest
{
    private readonly Mock<IFriendshipService> _friendshipServiceMock;
    private readonly Mock<ILogger<FriendsController>> _loggerMock;
    private readonly FriendsController _controller;
    private readonly Guid _currentUserId;

    /// <summary>
    /// Initializes a new instance of the FriendsControllerTest class.
    /// Sets up mocks and the controller context with a valid user ID claim.
    /// </summary>
    public FriendsControllerTest()
    {
        _friendshipServiceMock = new Mock<IFriendshipService>();
        _loggerMock = new Mock<ILogger<FriendsController>>();
        _controller = new FriendsController(_friendshipServiceMock.Object, _loggerMock.Object);

        _currentUserId = Guid.NewGuid();

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier, _currentUserId.ToString())
                ]))
            }
        };
    }

    /// <summary>
    /// Retrieves the value of a specified property from an object.
    /// </summary>
    /// <param name="src">The source object.</param>
    /// <param name="propName">The name of the property to retrieve.</param>
    /// <returns>The value of the property, or null if the property does not exist.</returns>
    private static object GetPropertyValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName)?.GetValue(src, null);
    }

    /// <summary>
    /// Tests that SendFriendRequest returns OkObjectResult when the request is successful.
    /// </summary>
    [Fact]
    public async Task SendFriendRequest_ShouldReturnOk_WhenRequestIsSuccessful()
    {
        var addresseeId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = true, FriendshipId = Guid.NewGuid() };
        _friendshipServiceMock.Setup(s => s.SendFriendRequestAsync(_currentUserId, addresseeId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.SendFriendRequest(addresseeId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<FriendshipResultDto>(okResult.Value);
        Assert.True(actualResult.Success);
        Assert.Equal(expectedResult.FriendshipId, actualResult.FriendshipId);
    }

    /// <summary>
    /// Tests that SendFriendRequest returns BadRequestObjectResult when the request fails.
    /// </summary>
    [Fact]
    public async Task SendFriendRequest_ShouldReturnBadRequest_WhenRequestFails()
    {
        var addresseeId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "Already friends" };
        _friendshipServiceMock.Setup(s => s.SendFriendRequestAsync(_currentUserId, addresseeId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.SendFriendRequest(addresseeId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        var actualMessage = GetPropertyValue(badRequestResult.Value, "Message");
        Assert.Equal(expectedResult.Message, actualMessage);
    }

    /// <summary>
    /// Tests that AcceptFriendRequest returns OkObjectResult when the acceptance is successful.
    /// </summary>
    [Fact]
    public async Task AcceptFriendRequest_ShouldReturnOk_WhenAcceptanceIsSuccessful()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = true, Status = FriendshipStatus.Accepted };
        _friendshipServiceMock.Setup(s => s.AcceptFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcceptFriendRequest(friendshipId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<FriendshipResultDto>(okResult.Value);
        Assert.True(actualResult.Success);
        Assert.Equal(expectedResult.Status, actualResult.Status);
    }

    /// <summary>
    /// Tests that AcceptFriendRequest returns ForbidResult when the user is not authorized.
    /// </summary>
    [Fact]
    public async Task AcceptFriendRequest_ShouldReturnForbid_WhenNotAuthorized()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "User not authorized" };
        _friendshipServiceMock.Setup(s => s.AcceptFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcceptFriendRequest(friendshipId);

        Assert.IsType<ForbidResult>(result);
    }

    /// <summary>
    /// Tests that AcceptFriendRequest returns NotFoundObjectResult when the friendship is not found.
    /// </summary>
    [Fact]
    public async Task AcceptFriendRequest_ShouldReturnNotFound_WhenFriendshipNotFound()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "Friendship not found" };
        _friendshipServiceMock.Setup(s => s.AcceptFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcceptFriendRequest(friendshipId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
        var actualMessage = GetPropertyValue(notFoundResult.Value, "Message");
        Assert.Equal(expectedResult.Message, actualMessage);
    }

    /// <summary>
    /// Tests that AcceptFriendRequest returns BadRequestObjectResult for other failures.
    /// </summary>
    [Fact]
    public async Task AcceptFriendRequest_ShouldReturnBadRequest_ForOtherFailures()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "Invalid state" };
        _friendshipServiceMock.Setup(s => s.AcceptFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcceptFriendRequest(friendshipId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        var actualMessage = GetPropertyValue(badRequestResult.Value, "Message");
        Assert.Equal(expectedResult.Message, actualMessage);
    }

    /// <summary>
    /// Tests that DeclineFriendRequest returns OkObjectResult when the request is declined successfully.
    /// </summary>
    [Fact]
    public async Task DeclineFriendRequest_ShouldReturnOk_WhenDeclinedSuccessfully()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = true, Status = FriendshipStatus.Declined };
        _friendshipServiceMock.Setup(s => s.DeclineFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.DeclineFriendRequest(friendshipId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<FriendshipResultDto>(okResult.Value);
        Assert.True(actualResult.Success);
        Assert.Equal(expectedResult.Status, actualResult.Status);
    }

    /// <summary>
    /// Tests that DeclineFriendRequest returns ForbidResult when the user is not authorized.
    /// </summary>
    [Fact]
    public async Task DeclineFriendRequest_ShouldReturnForbid_WhenDeclineNotAuthorized()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "User not authorized" };
        _friendshipServiceMock.Setup(s => s.DeclineFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.DeclineFriendRequest(friendshipId);

        Assert.IsType<ForbidResult>(result);
    }

    /// <summary>
    /// Tests that DeclineFriendRequest returns NotFoundObjectResult when the friendship is not found.
    /// </summary>
    [Fact]
    public async Task DeclineFriendRequest_ShouldReturnNotFound_WhenDeclineFriendshipNotFound()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "Friendship not found" };
        _friendshipServiceMock.Setup(s => s.DeclineFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.DeclineFriendRequest(friendshipId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
        var actualMessage = GetPropertyValue(notFoundResult.Value, "Message");
        Assert.Equal(expectedResult.Message, actualMessage);
    }

    /// <summary>
    /// Tests that DeclineFriendRequest returns BadRequestObjectResult for other failures.
    /// </summary>
    [Fact]
    public async Task DeclineFriendRequest_ShouldReturnBadRequest_ForOtherDeclineFailures()
    {
        var friendshipId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto
            { Success = false, Message = "Cannot decline accepted request" };
        _friendshipServiceMock.Setup(s => s.DeclineFriendRequestAsync(friendshipId, _currentUserId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.DeclineFriendRequest(friendshipId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        var actualMessage = GetPropertyValue(badRequestResult.Value, "Message");
        Assert.Equal(expectedResult.Message, actualMessage);
    }

    /// <summary>
    /// Tests that RemoveFriend returns OkObjectResult when the friend is removed successfully.
    /// </summary>
    [Fact]
    public async Task RemoveFriend_ShouldReturnOk_WhenRemovedSuccessfully()
    {
        var friendId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = true, Message = "Friend removed." };
        _friendshipServiceMock.Setup(s => s.RemoveFriendAsync(_currentUserId, friendId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.RemoveFriend(friendId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<FriendshipResultDto>(okResult.Value);
        Assert.True(actualResult.Success);
        Assert.Equal(expectedResult.Message, actualResult.Message);
    }

    /// <summary>
    /// Tests that RemoveFriend returns NotFoundObjectResult when the friendship is not found.
    /// </summary>
    [Fact]
    public async Task RemoveFriend_ShouldReturnNotFound_WhenFriendshipNotFound()
    {
        var friendId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "Friendship not found" };
        _friendshipServiceMock.Setup(s => s.RemoveFriendAsync(_currentUserId, friendId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.RemoveFriend(friendId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
        var actualMessage = GetPropertyValue(notFoundResult.Value, "Message");
        Assert.Equal(expectedResult.Message, actualMessage);
    }

    /// <summary>
    /// Tests that RemoveFriend returns BadRequestObjectResult for other failures.
    /// </summary>
    [Fact]
    public async Task RemoveFriend_ShouldReturnBadRequest_ForOtherFailures()
    {
        var friendId = Guid.NewGuid();
        var expectedResult = new FriendshipResultDto { Success = false, Message = "Cannot remove self" };
        _friendshipServiceMock.Setup(s => s.RemoveFriendAsync(_currentUserId, friendId))
            .ReturnsAsync(expectedResult);

        var result = await _controller.RemoveFriend(friendId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
        var actualMessage = GetPropertyValue(badRequestResult.Value, "Message");
        Assert.Equal(expectedResult.Message, actualMessage);
    }

    /// <summary>
    /// Tests that GetFriends returns OkObjectResult with a list of friends.
    /// </summary>
    [Fact]
    public async Task GetFriends_ShouldReturnOkWithFriendsList()
    {
        var friends = new List<FriendDto>
        {
            new()
            {
                Id = Guid.NewGuid(), Name = "Friend1", Tag = "F1", FriendshipId = Guid.NewGuid(),
                BecameFriendsAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "Friend2", Tag = "F2", FriendshipId = Guid.NewGuid(),
                BecameFriendsAt = DateTime.UtcNow
            }
        };
        _friendshipServiceMock.Setup(s => s.GetFriendsAsync(_currentUserId)).ReturnsAsync(friends);

        var result = await _controller.GetFriends();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualFriends = Assert.IsType<List<FriendDto>>(okResult.Value);
        Assert.Equal(friends.Count, actualFriends.Count);
        Assert.Equal(friends[0].Id, actualFriends[0].Id);
    }

    /// <summary>
    /// Tests that GetFriends returns OkObjectResult with an empty list when there are no friends.
    /// </summary>
    [Fact]
    public async Task GetFriends_ShouldReturnOkWithEmptyList_WhenNoFriends()
    {
        _friendshipServiceMock.Setup(s => s.GetFriendsAsync(_currentUserId)).ReturnsAsync(new List<FriendDto>());

        var result = await _controller.GetFriends();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualFriends = Assert.IsType<List<FriendDto>>(okResult.Value);
        Assert.Empty(actualFriends);
    }

    /// <summary>
    /// Tests that GetPendingIncomingRequests returns OkObjectResult with a list of incoming friend requests.
    /// </summary>
    [Fact]
    public async Task GetPendingIncomingRequests_ShouldReturnOkWithRequestsList()
    {
        var requests = new List<FriendRequestDto>
        {
            new() { Id = Guid.NewGuid(), RequesterName = "Req1", RequestedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), RequesterName = "Req2", RequestedAt = DateTime.UtcNow }
        };
        _friendshipServiceMock.Setup(s => s.GetPendingIncomingRequestsAsync(_currentUserId)).ReturnsAsync(requests);

        var result = await _controller.GetPendingIncomingRequests();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualRequests = Assert.IsType<List<FriendRequestDto>>(okResult.Value);
        Assert.Equal(requests.Count, actualRequests.Count);
        Assert.Equal(requests[0].Id, actualRequests[0].Id);
    }

    /// <summary>
    /// Tests that GetPendingIncomingRequests returns OkObjectResult with an empty list when there are no incoming requests.
    /// </summary>
    [Fact]
    public async Task GetPendingIncomingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests()
    {
        _friendshipServiceMock.Setup(s => s.GetPendingIncomingRequestsAsync(_currentUserId))
            .ReturnsAsync(new List<FriendRequestDto>());

        var result = await _controller.GetPendingIncomingRequests();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualRequests = Assert.IsType<List<FriendRequestDto>>(okResult.Value);
        Assert.Empty(actualRequests);
    }

    /// <summary>
    /// Tests that GetPendingOutgoingRequests returns OkObjectResult with a list of outgoing friend requests.
    /// </summary>
    [Fact]
    public async Task GetPendingOutgoingRequests_ShouldReturnOkWithRequestsList()
    {
        var requests = new List<FriendRequestDto>
        {
            new() { Id = Guid.NewGuid(), AddresseeName = "Add1", RequestedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), AddresseeName = "Add2", RequestedAt = DateTime.UtcNow }
        };
        _friendshipServiceMock.Setup(s => s.GetPendingOutgoingRequestsAsync(_currentUserId)).ReturnsAsync(requests);

        var result = await _controller.GetPendingOutgoingRequests();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualRequests = Assert.IsType<List<FriendRequestDto>>(okResult.Value);
        Assert.Equal(requests.Count, actualRequests.Count);
        Assert.Equal(requests[0].Id, actualRequests[0].Id);
    }

    /// <summary>
    /// Tests that GetPendingOutgoingRequests returns OkObjectResult with an empty list when there are no outgoing requests.
    /// </summary>
    [Fact]
    public async Task GetPendingOutgoingRequests_ShouldReturnOkWithEmptyList_WhenNoRequests()
    {
        _friendshipServiceMock.Setup(s => s.GetPendingOutgoingRequestsAsync(_currentUserId))
            .ReturnsAsync(new List<FriendRequestDto>());

        var result = await _controller.GetPendingOutgoingRequests();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualRequests = Assert.IsType<List<FriendRequestDto>>(okResult.Value);
        Assert.Empty(actualRequests);
    }

    /// <summary>
    /// Tests that GetFriendshipStatus returns "Self" status when checking the current user's own ID.
    /// </summary>
    [Fact]
    public async Task GetFriendshipStatus_ShouldReturnSelfStatus_WhenCheckingOwnId()
    {
        var result = await _controller.GetFriendshipStatus(_currentUserId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Equal("Self", GetPropertyValue(okResult.Value, "Status"));
        Assert.Equal("This is your own user ID.", GetPropertyValue(okResult.Value, "Message"));

        _friendshipServiceMock.Verify(s => s.GetFriendshipStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
            Times.Never);
    }

    /// <summary>
    /// Tests that GetFriendshipStatus returns "None" status when no friendship exists.
    /// </summary>
    [Fact]
    public async Task GetFriendshipStatus_ShouldReturnNoneStatus_WhenNoFriendshipExists()
    {
        var otherUserId = Guid.NewGuid();
        _friendshipServiceMock.Setup(s => s.GetFriendshipStatusAsync(_currentUserId, otherUserId))
            .ReturnsAsync((FriendshipStatus?)null);

        var result = await _controller.GetFriendshipStatus(otherUserId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Equal("None", GetPropertyValue(okResult.Value, "Status"));
        Assert.Equal("No friendship record exists.", GetPropertyValue(okResult.Value, "Message"));
    }

    /// <summary>
    /// Tests that GetFriendshipStatus returns the correct status when a friendship exists.
    /// </summary>
    [Fact]
    public async Task GetFriendshipStatus_ShouldReturnCorrectStatus_WhenFriendshipExists()
    {
        // Arrange: Set up a mock friendship status and user ID.
        var otherUserId = Guid.NewGuid();
        _friendshipServiceMock.Setup(s => s.GetFriendshipStatusAsync(_currentUserId, otherUserId))
            .ReturnsAsync(FriendshipStatus.Accepted);

        // Act: Call the GetFriendshipStatus method on the controller.
        var result = await _controller.GetFriendshipStatus(otherUserId);

        // Assert: Verify the result is of type OkObjectResult and contains the expected status.
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Equal("Accepted", GetPropertyValue(okResult.Value, "Status"));
        Assert.Equal(FriendshipStatus.Accepted, GetPropertyValue(okResult.Value, "RawStatus"));
    }

    /// <summary>
    /// Tests that all controller methods throw an InvalidOperationException when the User ID claim is invalid.
    /// </summary>
    [Fact]
    public async Task ControllerMethods_ShouldThrowInvalidOperationException_WhenUserIdClaimIsInvalid()
    {
        // Arrange: Create a controller instance with an invalid User ID claim.
        var controllerWithInvalidClaim = new FriendsController(_friendshipServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity([
                        new Claim(ClaimTypes.NameIdentifier, "not-a-guid")
                    ]))
                }
            }
        };

        // Act & Assert: Verify that each controller method throws an InvalidOperationException.
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.SendFriendRequest(Guid.NewGuid()));
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.AcceptFriendRequest(Guid.NewGuid()));
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.DeclineFriendRequest(Guid.NewGuid()));
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.RemoveFriend(Guid.NewGuid()));
        await Assert.ThrowsAsync<InvalidOperationException>(() => controllerWithInvalidClaim.GetFriends());
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.GetPendingIncomingRequests());
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.GetPendingOutgoingRequests());
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.GetFriendshipStatus(Guid.NewGuid()));
    }

    /// <summary>
    /// Tests that an error is logged when the User ID claim is invalid.
    /// </summary>
    [Fact]
    public async Task ControllerMethods_ShouldLogError_WhenUserIdClaimIsInvalid()
    {
        // Arrange: Create a controller instance with an invalid User ID claim.
        var controllerWithInvalidClaim = new FriendsController(_friendshipServiceMock.Object, _loggerMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity([
                        new Claim(ClaimTypes.NameIdentifier, "not-a-guid")
                    ]))
                }
            }
        };

        // Act & Assert: Verify that an InvalidOperationException is thrown and an error is logged.
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            controllerWithInvalidClaim.SendFriendRequest(Guid.NewGuid()));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unable to parse User ID from token claims.")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }
}