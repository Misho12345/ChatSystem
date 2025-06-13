using UserAccountService.Data;
using UserAccountService.Models;
using UserAccountService.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace UserAccountService.Services;

public class FriendshipService(UserAccountDbContext context, ILogger<FriendshipService> logger)
    : IFriendshipService
{
    public async Task<FriendshipResultDto> SendFriendRequestAsync(Guid requesterId, Guid addresseeId)
    {
        if (requesterId == addresseeId)
        {
            return new FriendshipResultDto { Success = false, Message = "Cannot send a friend request to yourself." };
        }

        var addresseeExists = await context.Users.AnyAsync(u => u.Id == addresseeId);
        if (!addresseeExists)
        {
            return new FriendshipResultDto { Success = false, Message = "Recipient user not found." };
        }

        var existingFriendship = await context.Friendships
            .FirstOrDefaultAsync(f =>
                (f.RequesterId == requesterId && f.AddresseeId == addresseeId) ||
                (f.RequesterId == addresseeId && f.AddresseeId == requesterId));

        if (existingFriendship != null)
        {
            return existingFriendship.Status switch
            {
                FriendshipStatus.Accepted => new FriendshipResultDto
                {
                    Success = false, Message = "You are already friends."
                },
                FriendshipStatus.Pending when existingFriendship.RequesterId == requesterId => new FriendshipResultDto
                {
                    Success = false, Message = "Friend request already sent."
                },
                FriendshipStatus.Pending when existingFriendship.AddresseeId == requesterId => new FriendshipResultDto
                {
                    Success = false,
                    Message = "This user has already sent you a friend request. Please respond to it."
                },
                FriendshipStatus.Blocked => new FriendshipResultDto
                {
                    Success = false, Message = "Unable to send friend request due to a block."
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = requesterId,
            AddresseeId = addresseeId,
            Status = FriendshipStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };

        try
        {
            context.Friendships.Add(friendship);
            await context.SaveChangesAsync();
            logger.LogInformation(
                "Friend request sent from {RequesterId} to {AddresseeId}. FriendshipId: {FriendshipId}", requesterId,
                addresseeId, friendship.Id);
            return new FriendshipResultDto
            {
                Success = true, Message = "Friend request sent.", FriendshipId = friendship.Id,
                Status = friendship.Status
            };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error sending friend request from {RequesterId} to {AddresseeId}.",
                requesterId, addresseeId);
            return new FriendshipResultDto { Success = false, Message = "Database error sending friend request." };
        }
    }

    public async Task<FriendshipResultDto> AcceptFriendRequestAsync(Guid friendshipId, Guid currentUserId)
    {
        var friendship = await context.Friendships.FindAsync(friendshipId);

        if (friendship == null)
        {
            return new FriendshipResultDto { Success = false, Message = "Friend request not found." };
        }

        if (friendship.AddresseeId != currentUserId)
        {
            return new FriendshipResultDto
                { Success = false, Message = "You are not authorized to accept this request." };
        }

        if (friendship.Status != FriendshipStatus.Pending)
        {
            return new FriendshipResultDto
                { Success = false, Message = $"Request is no longer pending (current status: {friendship.Status})." };
        }

        friendship.Status = FriendshipStatus.Accepted;
        friendship.RespondedAt = DateTime.UtcNow;

        try
        {
            context.Friendships.Update(friendship);
            await context.SaveChangesAsync();
            logger.LogInformation("Friend request {FriendshipId} accepted by {UserId}.", friendshipId, currentUserId);
            return new FriendshipResultDto
            {
                Success = true, Message = "Friend request accepted.", FriendshipId = friendship.Id,
                Status = friendship.Status
            };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error accepting friend request {FriendshipId}.", friendshipId);
            return new FriendshipResultDto { Success = false, Message = "Database error accepting friend request." };
        }
    }

    public async Task<FriendshipResultDto> DeclineFriendRequestAsync(Guid friendshipId, Guid currentUserId)
    {
        var friendship = await context.Friendships.FindAsync(friendshipId);

        if (friendship == null)
        {
            return new FriendshipResultDto { Success = false, Message = "Friend request not found." };
        }

        if (friendship.AddresseeId != currentUserId && friendship.RequesterId != currentUserId)
        {
            return new FriendshipResultDto
                { Success = false, Message = "You are not authorized to decline/cancel this request." };
        }

        if (friendship.Status != FriendshipStatus.Pending)
        {
            return new FriendshipResultDto
                { Success = false, Message = $"Request is no longer pending (current status: {friendship.Status})." };
        }

        friendship.Status = FriendshipStatus.Declined;
        friendship.RespondedAt = DateTime.UtcNow;

        try
        {
            context.Friendships.Update(friendship);
            await context.SaveChangesAsync();
            logger.LogInformation("Friend request {FriendshipId} declined/cancelled by {UserId}.", friendshipId,
                currentUserId);
            return new FriendshipResultDto
            {
                Success = true, Message = "Friend request declined/cancelled.", FriendshipId = friendship.Id,
                Status = friendship.Status
            };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error declining friend request {FriendshipId}.", friendshipId);
            return new FriendshipResultDto { Success = false, Message = "Database error declining friend request." };
        }
    }

    public async Task<FriendshipResultDto> RemoveFriendAsync(Guid userId, Guid friendId)
    {
        var friendship = await context.Friendships
            .FirstOrDefaultAsync(f =>
                f.Status == FriendshipStatus.Accepted &&
                ((f.RequesterId == userId && f.AddresseeId == friendId) ||
                 (f.RequesterId == friendId && f.AddresseeId == userId)));

        if (friendship == null)
        {
            return new FriendshipResultDto
                { Success = false, Message = "Friendship not found or you are not friends." };
        }

        context.Friendships.Remove(friendship);

        try
        {
            await context.SaveChangesAsync();
            logger.LogInformation(
                "Friendship between {UserId1} and {UserId2} (FriendshipId: {FriendshipId}) removed by {RemoverId}.",
                friendship.RequesterId, friendship.AddresseeId, friendship.Id, userId);
            return new FriendshipResultDto { Success = true, Message = "Friend removed." };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error removing friend for user {UserId} and friend {FriendId}.", userId,
                friendId);
            return new FriendshipResultDto { Success = false, Message = "Database error removing friend." };
        }
    }

    public async Task<IEnumerable<FriendDto>> GetFriendsAsync(Guid userId)
    {
        var friends = await context.Friendships
            .Where(f => f.Status == FriendshipStatus.Accepted && (f.RequesterId == userId || f.AddresseeId == userId))
            .Select(f => new FriendDto
            {
                Id = f.RequesterId == userId ? f.Addressee.Id : f.Requester.Id,
                Name = f.RequesterId == userId ? f.Addressee.Name : f.Requester.Name,
                Tag = f.RequesterId == userId ? f.Addressee.Tag : f.Requester.Tag,
                FriendshipId = f.Id,
                BecameFriendsAt = f.RespondedAt ?? f.RequestedAt
            })
            .ToListAsync();

        return friends;
    }

    public async Task<IEnumerable<FriendRequestDto>> GetPendingIncomingRequestsAsync(Guid userId)
    {
        var requests = await context.Friendships
            .Where(f => f.AddresseeId == userId && f.Status == FriendshipStatus.Pending)
            .Select(f => new FriendRequestDto
            {
                Id = f.Id,
                RequesterId = f.Requester.Id,
                RequesterName = f.Requester.Name,
                RequesterTag = f.Requester.Tag,
                RequestedAt = f.RequestedAt
            })
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();

        return requests;
    }

    public async Task<IEnumerable<FriendRequestDto>> GetPendingOutgoingRequestsAsync(Guid userId)
    {
        var requests = await context.Friendships
            .Where(f => f.RequesterId == userId && f.Status == FriendshipStatus.Pending)
            .Select(f => new FriendRequestDto
            {
                Id = f.Id,
                AddresseeId = f.Addressee.Id,
                AddresseeName = f.Addressee.Name,
                AddresseeTag = f.Addressee.Tag,
                RequestedAt = f.RequestedAt
            })
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();

        return requests;
    }


    public async Task<FriendshipStatus?> GetFriendshipStatusAsync(Guid userId1, Guid userId2)
    {
        var friendship = await context.Friendships
            .FirstOrDefaultAsync(f =>
                (f.RequesterId == userId1 && f.AddresseeId == userId2) ||
                (f.RequesterId == userId2 && f.AddresseeId == userId1));

        return friendship?.Status;
    }
}