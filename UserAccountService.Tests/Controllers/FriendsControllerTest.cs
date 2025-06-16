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

public class FriendsControllerTest
{
    private readonly Mock<IFriendshipService> _friendshipServiceMock;
    private readonly Mock<ILogger<FriendsController>> _loggerMock;
    private readonly FriendsController _controller;
    private readonly Guid _currentUserId;

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

    private static object GetPropertyValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName)?.GetValue(src, null);
    }

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

    [Fact]
    public async Task GetFriends_ShouldReturnOkWithEmptyList_WhenNoFriends()
    {
        _friendshipServiceMock.Setup(s => s.GetFriendsAsync(_currentUserId)).ReturnsAsync(new List<FriendDto>());

        var result = await _controller.GetFriends();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualFriends = Assert.IsType<List<FriendDto>>(okResult.Value);
        Assert.Empty(actualFriends);
    }

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

    [Fact]
    public async Task GetFriendshipStatus_ShouldReturnSelfStatus_WhenCheckingOwnId()
    {
        var otherUserId = _currentUserId;

        var result = await _controller.GetFriendshipStatus(otherUserId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Equal("Self", GetPropertyValue(okResult.Value, "Status"));
        Assert.Equal("This is your own user ID.", GetPropertyValue(okResult.Value, "Message"));

        _friendshipServiceMock.Verify(s => s.GetFriendshipStatusAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
            Times.Never);
    }

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

    [Fact]
    public async Task GetFriendshipStatus_ShouldReturnCorrectStatus_WhenFriendshipExists()
    {
        var otherUserId = Guid.NewGuid();
        _friendshipServiceMock.Setup(s => s.GetFriendshipStatusAsync(_currentUserId, otherUserId))
            .ReturnsAsync(FriendshipStatus.Accepted);

        var result = await _controller.GetFriendshipStatus(otherUserId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.Equal("Accepted", GetPropertyValue(okResult.Value, "Status"));
        Assert.Equal(FriendshipStatus.Accepted, GetPropertyValue(okResult.Value, "RawStatus"));
    }

    [Fact]
    public async Task ControllerMethods_ShouldThrowInvalidOperationException_WhenUserIdClaimIsInvalid()
    {
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

    [Fact]
    public async Task ControllerMethods_ShouldLogError_WhenUserIdClaimIsInvalid()
    {
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