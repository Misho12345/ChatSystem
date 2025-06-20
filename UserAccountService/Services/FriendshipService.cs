﻿using UserAccountService.Data;
using UserAccountService.Models;
using UserAccountService.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using UserAccountService.Hubs;

namespace UserAccountService.Services;

/// <summary>
/// Provides methods for managing friendships between users, including sending requests, accepting/declining requests, 
/// removing friends, and retrieving friendship-related data.
/// </summary>
public class FriendshipService(UserAccountDbContext context, ILogger<FriendshipService> logger, IHubContext<FriendshipHub> friendshipHubContext)
    : IFriendshipService
{
    /// <summary>
    /// Sends a friend request from one user to another.
    /// </summary>
    /// <param name="requesterId">The ID of the user sending the friend request.</param>
    /// <param name="addresseeId">The ID of the user receiving the friend request.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
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
                    Success = false,
                    Message = "You are already friends."
                },
                FriendshipStatus.Pending when existingFriendship.RequesterId == requesterId => new FriendshipResultDto
                {
                    Success = false,
                    Message = "Friend request already sent."
                },
                FriendshipStatus.Pending when existingFriendship.AddresseeId == requesterId => new FriendshipResultDto
                {
                    Success = false,
                    Message = "This user has already sent you a friend request. Please respond to it."
                },
                FriendshipStatus.Blocked => new FriendshipResultDto
                {
                    Success = false,
                    Message = "Unable to send friend request due to a block."
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

            var requester = await context.Users.FindAsync(requesterId);
            var requestDto = new FriendRequestDto
            {
                Id = friendship.Id,
                RequesterId = requester!.Id,
                RequesterName = requester.Name,
                RequesterTag = requester.Tag,
                RequestedAt = friendship.RequestedAt
            };
            await friendshipHubContext.Clients.Group(addresseeId.ToString()).SendAsync("NewFriendRequest", requestDto);

            return new FriendshipResultDto
            {
                Success = true,
                Message = "Friend request sent.",
                FriendshipId = friendship.Id,
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

    /// <summary>
    /// Accepts a pending friend request.
    /// </summary>
    /// <param name="friendshipId">The ID of the friendship to accept.</param>
    /// <param name="currentUserId">The ID of the user accepting the request.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
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

            await friendshipHubContext.Clients.Group(friendship.RequesterId.ToString()).SendAsync("FriendRequestProcessed");
            await friendshipHubContext.Clients.Group(friendship.AddresseeId.ToString()).SendAsync("FriendRequestProcessed");

            return new FriendshipResultDto
            {
                Success = true,
                Message = "Friend request accepted.",
                FriendshipId = friendship.Id,
                Status = friendship.Status
            };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error accepting friend request {FriendshipId}.", friendshipId);
            return new FriendshipResultDto { Success = false, Message = "Database error accepting friend request." };
        }
    }

    /// <summary>
    /// Declines or cancels a pending friend request.
    /// </summary>
    /// <param name="friendshipId">The ID of the friendship to decline or cancel.</param>
    /// <param name="currentUserId">The ID of the user declining or canceling the request.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
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

        context.Friendships.Remove(friendship);

        try
        {
            await context.SaveChangesAsync();
            logger.LogInformation("Friend request {FriendshipId} declined/cancelled by {UserId}.", friendshipId,
                currentUserId);

            await friendshipHubContext.Clients.Group(friendship.RequesterId.ToString()).SendAsync("FriendRequestProcessed");
            await friendshipHubContext.Clients.Group(friendship.AddresseeId.ToString()).SendAsync("FriendRequestProcessed");

            return new FriendshipResultDto
            {
                Success = true,
                Message = "Friend request declined/cancelled.",
                FriendshipId = friendship.Id,
                Status = FriendshipStatus.Declined
            };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error declining friend request {FriendshipId}.", friendshipId);
            return new FriendshipResultDto { Success = false, Message = "Database error declining friend request." };
        }
    }

    /// <summary>
    /// Removes an existing friendship between two users.
    /// </summary>
    /// <param name="userId">The ID of the user initiating the removal.</param>
    /// <param name="friendId">The ID of the friend to remove.</param>
    /// <returns>A <see cref="FriendshipResultDto"/> containing the result of the operation.</returns>
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

        var otherUserId = friendship.RequesterId == userId ? friendship.AddresseeId : friendship.RequesterId;

        context.Friendships.Remove(friendship);

        try
        {
            await context.SaveChangesAsync();
            logger.LogInformation(
                "Friendship between {UserId1} and {UserId2} (FriendshipId: {FriendshipId}) removed by {RemoverId}.",
                friendship.RequesterId, friendship.AddresseeId, friendship.Id, userId);

            await friendshipHubContext.Clients.Group(otherUserId.ToString()).SendAsync("FriendshipRemoved");

            return new FriendshipResultDto { Success = true, Message = "Friend removed." };
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error removing friend for user {UserId} and friend {FriendId}.", userId,
                friendId);
            return new FriendshipResultDto { Success = false, Message = "Database error removing friend." };
        }
    }

    /// <summary>
    /// Retrieves a list of friends for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user whose friends are being retrieved.</param>
    /// <returns>A collection of <see cref="FriendDto"/> representing the user's friends.</returns>
    public async Task<IEnumerable<FriendDto>> GetFriendsAsync(Guid userId)
    {
        var friends = await context.Friendships
            .Where(f => f.Status == FriendshipStatus.Accepted && (f.RequesterId == userId || f.AddresseeId == userId))
            .Select(f => new FriendDto
            {
                Id = f.RequesterId == userId ? f.Addressee!.Id : f.Requester!.Id,
                Name = f.RequesterId == userId ? f.Addressee!.Name : f.Requester!.Name,
                Tag = f.RequesterId == userId ? f.Addressee!.Tag : f.Requester!.Tag,
                FriendshipId = f.Id,
                BecameFriendsAt = f.RespondedAt ?? f.RequestedAt
            })
            .ToListAsync();

        return friends;
    }

    /// <summary>
    /// Retrieves a list of incoming friend requests for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user whose incoming requests are being retrieved.</param>
    /// <returns>A collection of <see cref="FriendRequestDto"/> representing the incoming requests.</returns>
    public async Task<IEnumerable<FriendRequestDto>> GetPendingIncomingRequestsAsync(Guid userId)
    {
        var requests = await context.Friendships
            .Where(f => f.AddresseeId == userId && f.Status == FriendshipStatus.Pending)
            .Select(f => new FriendRequestDto
            {
                Id = f.Id,
                RequesterId = f.Requester!.Id,
                RequesterName = f.Requester.Name,
                RequesterTag = f.Requester.Tag,
                RequestedAt = f.RequestedAt
            })
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();

        return requests;
    }

    /// <summary>
    /// Retrieves a list of outgoing friend requests for a given user.
    /// </summary>
    /// <param name="userId">The ID of the user whose outgoing requests are being retrieved.</param>
    /// <returns>A collection of <see cref="FriendRequestDto"/> representing the outgoing requests.</returns>
    public async Task<IEnumerable<FriendRequestDto>> GetPendingOutgoingRequestsAsync(Guid userId)
    {
        var requests = await context.Friendships
            .Where(f => f.RequesterId == userId && f.Status == FriendshipStatus.Pending)
            .Select(f => new FriendRequestDto
            {
                Id = f.Id,
                AddresseeId = f.Addressee!.Id,
                AddresseeName = f.Addressee.Name,
                AddresseeTag = f.Addressee.Tag,
                RequestedAt = f.RequestedAt
            })
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();

        return requests;
    }


    /// <summary>
    /// Retrieves the friendship status between two users.
    /// </summary>
    /// <param name="userId1">The ID of the first user.</param>
    /// <param name="userId2">The ID of the second user.</param>
    /// <returns>The <see cref="FriendshipStatus"/> representing the status of the friendship, or null if no friendship exists.</returns>
    public async Task<FriendshipStatus?> GetFriendshipStatusAsync(Guid userId1, Guid userId2)
    {
        var friendship = await context.Friendships
            .FirstOrDefaultAsync(f =>
                (f.RequesterId == userId1 && f.AddresseeId == userId2) ||
                (f.RequesterId == userId2 && f.AddresseeId == userId1));

        return friendship?.Status;
    }
}