using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using UserAccountService.Data;
using UserAccountService.Models;
using UserAccountService.Models.DTOs;
using UserAccountService.Services;
using Xunit;

namespace UserAccountService.Tests.Services;

public class FriendshipServiceTests
{
    private UserAccountDbContext CreateContext(string name)
    {
        var options = new DbContextOptionsBuilder<UserAccountDbContext>()
            .UseInMemoryDatabase(name)
            .Options;
        return new UserAccountDbContext(options);
    }

    [Fact]
    public async Task SendFriendRequest_Succeeds_WhenValid()
    {
        await using var ctx = CreateContext(nameof(SendFriendRequest_Succeeds_WhenValid));
        var user1 = new User { Id = Guid.NewGuid(), Name = "A", Tag = "A#0001", Email = "a@test", PasswordHash = "h" };
        var user2 = new User { Id = Guid.NewGuid(), Name = "B", Tag = "B#0001", Email = "b@test", PasswordHash = "h" };
        ctx.Users.AddRange(user1, user2);
        await ctx.SaveChangesAsync();

        var svc = new FriendshipService(ctx, new NullLogger<FriendshipService>());
        var result = await svc.SendFriendRequestAsync(user1.Id, user2.Id);

        Assert.True(result.Success);
        Assert.Equal(FriendshipStatus.Pending, result.Status);
        Assert.NotNull(ctx.Friendships.SingleOrDefault(f => f.RequesterId == user1.Id && f.AddresseeId == user2.Id));
    }

    [Fact]
    public async Task SendFriendRequest_Fails_WhenSelfRequest()
    {
        await using var ctx = CreateContext(nameof(SendFriendRequest_Fails_WhenSelfRequest));
        var user = new User { Id = Guid.NewGuid(), Name = "A", Tag = "A#0001", Email = "a@test", PasswordHash = "h" };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        var svc = new FriendshipService(ctx, new NullLogger<FriendshipService>());
        var result = await svc.SendFriendRequestAsync(user.Id, user.Id);

        Assert.False(result.Success);
        Assert.Contains("yourself", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AcceptFriendRequest_Succeeds_WhenAuthorized()
    {
        await using var ctx = CreateContext(nameof(AcceptFriendRequest_Succeeds_WhenAuthorized));
        var requester = new User { Id = Guid.NewGuid(), Name = "A", Tag = "A#0001", Email = "a@test", PasswordHash = "h" };
        var addressee = new User { Id = Guid.NewGuid(), Name = "B", Tag = "B#0001", Email = "b@test", PasswordHash = "h" };
        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = requester.Id,
            AddresseeId = addressee.Id,
            Status = FriendshipStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };
        ctx.Users.AddRange(requester, addressee);
        ctx.Friendships.Add(friendship);
        await ctx.SaveChangesAsync();

        var svc = new FriendshipService(ctx, new NullLogger<FriendshipService>());
        var result = await svc.AcceptFriendRequestAsync(friendship.Id, addressee.Id);

        Assert.True(result.Success);
        var updated = await ctx.Friendships.FindAsync(friendship.Id);
        Assert.Equal(FriendshipStatus.Accepted, updated!.Status);
        Assert.NotNull(updated.RespondedAt);
    }

    [Fact]
    public async Task DeclineFriendRequest_Succeeds_ForRequesterOrAddressee()
    {
        await using var ctx = CreateContext(nameof(DeclineFriendRequest_Succeeds_ForRequesterOrAddressee));
        var u1 = new User { Id = Guid.NewGuid(), Name = "A", Tag = "A#0001", Email = "a@test", PasswordHash = "h" };
        var u2 = new User { Id = Guid.NewGuid(), Name = "B", Tag = "B#0001", Email = "b@test", PasswordHash = "h" };
        var f = new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = u1.Id,
            AddresseeId = u2.Id,
            Status = FriendshipStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };
        ctx.Users.AddRange(u1, u2);
        ctx.Friendships.Add(f);
        await ctx.SaveChangesAsync();

        var svc = new FriendshipService(ctx, new NullLogger<FriendshipService>());
        var resultByAddressee = await svc.DeclineFriendRequestAsync(f.Id, u2.Id);
        Assert.True(resultByAddressee.Success);

        f.Status = FriendshipStatus.Pending;
        ctx.Friendships.Update(f);
        await ctx.SaveChangesAsync();

        var resultByRequester = await svc.DeclineFriendRequestAsync(f.Id, u1.Id);
        Assert.True(resultByRequester.Success);
    }

    [Fact]
    public async Task GetFriends_ReturnsAcceptedFriends()
    {
        await using var ctx = CreateContext(nameof(GetFriends_ReturnsAcceptedFriends));
        var u1 = new User { Id = Guid.NewGuid(), Name = "A", Tag = "A#0001", Email = "a@test", PasswordHash = "h" };
        var u2 = new User { Id = Guid.NewGuid(), Name = "B", Tag = "B#0001", Email = "b@test", PasswordHash = "h" };
        var accepted = new Friendship
        {
            Id = Guid.NewGuid(),
            RequesterId = u1.Id,
            AddresseeId = u2.Id,
            Status = FriendshipStatus.Accepted,
            RequestedAt = DateTime.UtcNow.AddDays(-1),
            RespondedAt = DateTime.UtcNow
        };
        ctx.Users.AddRange(u1, u2);
        ctx.Friendships.Add(accepted);
        await ctx.SaveChangesAsync();

        var svc = new FriendshipService(ctx, new NullLogger<FriendshipService>());
        var friends = await svc.GetFriendsAsync(u1.Id);
        var friendDtos = friends as FriendDto[] ?? friends.ToArray();
            
        Assert.Single(friendDtos);
        Assert.Equal(u2.Id, friendDtos.First().Id);
        Assert.Equal(accepted.Id, friendDtos.First().FriendshipId);
    }
}