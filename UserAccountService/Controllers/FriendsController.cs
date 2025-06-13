using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAccountService.Services;
using System.Security.Claims;

namespace UserAccountService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FriendsController(IFriendshipService friendshipService, ILogger<FriendsController> logger)
    : ControllerBase
{
    private Guid GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }

        logger.LogError("Unable to parse User ID from token claims.");
        throw new InvalidOperationException("User ID could not be determined from the token.");
    }

    [HttpPost("request/{addresseeId:guid}")] // POST /api/friends/request/{guid}
    public async Task<IActionResult> SendFriendRequest(Guid addresseeId)
    {
        var requesterId = GetCurrentUserId();
        logger.LogInformation("User {RequesterId} attempting to send friend request to {AddresseeId}", requesterId,
            addresseeId);

        var result = await friendshipService.SendFriendRequestAsync(requesterId, addresseeId);

        if (!result.Success)
        {
            logger.LogWarning("Failed to send friend request from {RequesterId} to {AddresseeId}: {Message}",
                requesterId, addresseeId, result.Message);
            return BadRequest(new { result.Message });
        }

        logger.LogInformation(
            "Friend request successfully sent from {RequesterId} to {AddresseeId}. FriendshipId: {FriendshipId}",
            requesterId, addresseeId, result.FriendshipId);
        return Ok(result);
    }

    [HttpPost("accept/{friendshipId:guid}")] // POST /api/friends/accept/{guid}
    public async Task<IActionResult> AcceptFriendRequest(Guid friendshipId)
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("User {CurrentUserId} attempting to accept friend request {FriendshipId}", currentUserId,
            friendshipId);

        var result = await friendshipService.AcceptFriendRequestAsync(friendshipId, currentUserId);

        if (!result.Success)
        {
            logger.LogWarning("Failed to accept friend request {FriendshipId} by user {CurrentUserId}: {Message}",
                friendshipId, currentUserId, result.Message);
            if (result.Message.Contains("not authorized")) return Forbid();
            if (result.Message.Contains("not found")) return NotFound(new { result.Message });
            return BadRequest(new { result.Message });
        }

        logger.LogInformation("Friend request {FriendshipId} successfully accepted by user {CurrentUserId}",
            friendshipId, currentUserId);
        return Ok(result);
    }

    [HttpPost("decline/{friendshipId:guid}")] // POST /api/friends/decline/{guid}
    public async Task<IActionResult> DeclineFriendRequest(Guid friendshipId)
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("User {CurrentUserId} attempting to decline/cancel friend request {FriendshipId}",
            currentUserId, friendshipId);

        var result = await friendshipService.DeclineFriendRequestAsync(friendshipId, currentUserId);

        if (!result.Success)
        {
            logger.LogWarning(
                "Failed to decline/cancel friend request {FriendshipId} by user {CurrentUserId}: {Message}",
                friendshipId, currentUserId, result.Message);
            if (result.Message.Contains("not authorized")) return Forbid();
            if (result.Message.Contains("not found")) return NotFound(new { result.Message });
            return BadRequest(new { result.Message });
        }

        logger.LogInformation("Friend request {FriendshipId} successfully declined/cancelled by user {CurrentUserId}",
            friendshipId, currentUserId);
        return Ok(result);
    }

    // DELETE /api/friends/remove/{guid}
    public async Task<IActionResult> RemoveFriend(Guid friendId)
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("User {CurrentUserId} attempting to remove friend {FriendId}", currentUserId, friendId);

        var result = await friendshipService.RemoveFriendAsync(currentUserId, friendId);

        if (!result.Success)
        {
            logger.LogWarning("Failed to remove friend {FriendId} for user {CurrentUserId}: {Message}", friendId,
                currentUserId, result.Message);
            if (result.Message.Contains("not found")) return NotFound(new { result.Message });
            return BadRequest(new { result.Message });
        }

        logger.LogInformation("Friend {FriendId} successfully removed by user {CurrentUserId}", friendId,
            currentUserId);
        return Ok(result);
    }

    [HttpGet] // GET /api/friends
    public async Task<IActionResult> GetFriends()
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("Fetching friends list for user {CurrentUserId}", currentUserId);

        var friends = await friendshipService.GetFriendsAsync(currentUserId);
        return Ok(friends);
    }

    [HttpGet("requests/pending/incoming")] // GET /api/friends/requests/pending/incoming
    public async Task<IActionResult> GetPendingIncomingRequests()
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("Fetching incoming pending friend requests for user {CurrentUserId}", currentUserId);

        var requests = await friendshipService.GetPendingIncomingRequestsAsync(currentUserId);
        return Ok(requests);
    }

    [HttpGet("requests/pending/outgoing")] // GET /api/friends/requests/pending/outgoing
    public async Task<IActionResult> GetPendingOutgoingRequests()
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("Fetching outgoing pending friend requests for user {CurrentUserId}", currentUserId);

        var requests = await friendshipService.GetPendingOutgoingRequestsAsync(currentUserId);
        return Ok(requests);
    }

    [HttpGet("status/{otherUserId:guid}")] // GET /api/friends/status/{guid}
    public async Task<IActionResult> GetFriendshipStatus(Guid otherUserId)
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("Fetching friendship status between user {CurrentUserId} and {OtherUserId}",
            currentUserId, otherUserId);

        if (currentUserId == otherUserId)
        {
            return Ok(new { Status = "Self", Message = "This is your own user ID." });
        }

        var status = await friendshipService.GetFriendshipStatusAsync(currentUserId, otherUserId);

        return status == null
            ? Ok(new { Status = "None", Message = "No friendship record exists." })
            : Ok(new { Status = status.ToString(), RawStatus = status });
    }
}