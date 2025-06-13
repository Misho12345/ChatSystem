using UserAccountService.Models;
using UserAccountService.Models.DTOs;

namespace UserAccountService.Services;

public interface IFriendshipService
{
    Task<FriendshipResultDto> SendFriendRequestAsync(Guid requesterId, Guid addresseeId);
    Task<FriendshipResultDto> AcceptFriendRequestAsync(Guid friendshipId, Guid currentUserId);
    Task<FriendshipResultDto> DeclineFriendRequestAsync(Guid friendshipId, Guid currentUserId);
    Task<FriendshipResultDto> RemoveFriendAsync(Guid userId, Guid friendId);
    Task<IEnumerable<FriendDto>> GetFriendsAsync(Guid userId);
    Task<IEnumerable<FriendRequestDto>> GetPendingIncomingRequestsAsync(Guid userId);
    Task<IEnumerable<FriendRequestDto>> GetPendingOutgoingRequestsAsync(Guid userId);
    Task<FriendshipStatus?> GetFriendshipStatusAsync(Guid userId1, Guid userId2);
}