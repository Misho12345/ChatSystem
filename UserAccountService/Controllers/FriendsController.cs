using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAccountService.Services;
using System.Security.Claims;

namespace UserAccountService.Controllers;

/// <summary>
/// Manages user friend relationships.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FriendsController(IFriendshipService friendshipService, ILogger<FriendsController> logger)
    : ControllerBase
{
    /// <summary>
    /// Retrieves the current user's ID from the token claims.
    /// </summary>
    /// <returns>The unique identifier of the current user.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the user ID cannot be determined from the token.</exception>
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

    /// <summary>
    /// Sends a friend request to another user.
    /// </summary>
    /// <param name="addresseeId">The unique identifier of the user receiving the friend request.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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

    /// <summary>
    /// Accepts a pending friend request.
    /// </summary>
    /// <param name="friendshipId">The unique identifier of the friendship.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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

    /// <summary>
    /// Declines or cancels a pending friend request.
    /// </summary>
    /// <param name="friendshipId">The unique identifier of the friendship.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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

    /// <summary>
    /// Removes a friend from the current user's friend list.
    /// </summary>
    /// <param name="friendId">The unique identifier of the friend to remove.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("remove/{friendId:guid}")]
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

    /// <summary>
    /// Retrieves the current user's list of friends.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of friends.</returns>
    [HttpGet] // GET /api/friends
    public async Task<IActionResult> GetFriends()
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("Fetching friends list for user {CurrentUserId}", currentUserId);

        var friends = await friendshipService.GetFriendsAsync(currentUserId);
        return Ok(friends);
    }

    /// <summary>
    /// Retrieves the current user's incoming pending friend requests.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of incoming pending friend requests.</returns>
    [HttpGet("requests/pending/incoming")] // GET /api/friends/requests/pending/incoming
    public async Task<IActionResult> GetPendingIncomingRequests()
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("Fetching incoming pending friend requests for user {CurrentUserId}", currentUserId);

        var requests = await friendshipService.GetPendingIncomingRequestsAsync(currentUserId);
        return Ok(requests);
    }

    /// <summary>
    /// Retrieves the current user's outgoing pending friend requests.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of outgoing pending friend requests.</returns>
    [HttpGet("requests/pending/outgoing")] // GET /api/friends/requests/pending/outgoing
    public async Task<IActionResult> GetPendingOutgoingRequests()
    {
        var currentUserId = GetCurrentUserId();
        logger.LogInformation("Fetching outgoing pending friend requests for user {CurrentUserId}", currentUserId);

        var requests = await friendshipService.GetPendingOutgoingRequestsAsync(currentUserId);
        return Ok(requests);
    }

    /// <summary>
    /// Retrieves the friendship status between the current user and another user.
    /// </summary>
    /// <param name="otherUserId">The unique identifier of the other user.</param>
    /// <returns>An <see cref="IActionResult"/> containing the friendship status.</returns>
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
