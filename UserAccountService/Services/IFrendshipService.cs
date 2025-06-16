using UserAccountService.Models;
using UserAccountService.Models.DTOs;

namespace UserAccountService.Services;

/// <summary>
/// Defines the contract for managing friendships between users.
/// </summary>
public interface IFriendshipService
{
    /// <summary>
    /// Sends a friend request from one user to another.
    /// </summary>
    /// <param name="requesterId">The ID of the user sending the friend request.</param>
    /// <param name="addresseeId">The ID of the user receiving the friend request.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
    Task<FriendshipResultDto> SendFriendRequestAsync(Guid requesterId, Guid addresseeId);

    /// <summary>
    /// Accepts a pending friend request.
    /// </summary>
    /// <param name="friendshipId">The ID of the friendship to accept.</param>
    /// <param name="currentUserId">The ID of the user accepting the request.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
    Task<FriendshipResultDto> AcceptFriendRequestAsync(Guid friendshipId, Guid currentUserId);

    /// <summary>
    /// Declines a pending friend request.
    /// </summary>
    /// <param name="friendshipId">The ID of the friendship to decline.</param>
    /// <param name="currentUserId">The ID of the user declining the request.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
    Task<FriendshipResultDto> DeclineFriendRequestAsync(Guid friendshipId, Guid currentUserId);

    /// <summary>
    /// Removes an existing friendship between two users.
    /// </summary>
    /// <param name="userId">The ID of the user initiating the removal.</param>
    /// <param name="friendId">The ID of the friend to remove.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
    Task<FriendshipResultDto> RemoveFriendAsync(Guid userId, Guid friendId);

    /// <summary>
    /// Retrieves a list of friends for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user whose friends are being retrieved.</param>
    /// <returns>A collection of <see cref="FriendDto"/> representing the user's friends.</returns>
    Task<IEnumerable<FriendDto>> GetFriendsAsync(Guid userId);

    /// <summary>
    /// Retrieves a list of incoming friend requests for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user whose incoming requests are being retrieved.</param>
    /// <returns>A collection of <see cref="FriendRequestDto"/> representing the incoming requests.</returns>
    Task<IEnumerable<FriendRequestDto>> GetPendingIncomingRequestsAsync(Guid userId);

    /// <summary>
    /// Retrieves a list of outgoing friend requests for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user whose outgoing requests are being retrieved.</param>
    /// <returns>A collection of <see cref="FriendRequestDto"/> representing the outgoing requests.</returns>
    Task<IEnumerable<FriendRequestDto>> GetPendingOutgoingRequestsAsync(Guid userId);

    /// <summary>
    /// Retrieves the friendship status between two users.
    /// </summary>
    /// <param name="userId1">The ID of the first user.</param>
    /// <param name="userId2">The ID of the second user.</param>
    /// <returns>The <see cref="FriendshipStatus"/> representing the status of the friendship, or null if no friendship exists.</returns>
    Task<FriendshipStatus?> GetFriendshipStatusAsync(Guid userId1, Guid userId2);
}