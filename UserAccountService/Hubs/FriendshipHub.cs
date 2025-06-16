using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace UserAccountService.Hubs;

/// <summary>
/// Represents a SignalR hub for managing friendships and user connections.
/// </summary>
[Authorize]
public class FriendshipHub : Hub
{
    /// <summary>
    /// Retrieves the unique identifier of the currently connected user.
    /// </summary>
    /// <returns>
    /// A <see cref="Guid"/> representing the user's unique identifier, or <see cref="Guid.Empty"/> if the identifier is invalid or not found.
    /// </returns>
    private Guid GetCurrentUserId()
    {
        var userIdString = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = Guid.TryParse(userIdString, out var userId);
        return userId;
    }

    /// <summary>
    /// Handles the event when a client connects to the hub.
    /// Adds the user to a SignalR group based on their unique identifier.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task OnConnectedAsync()
    {
        var userId = GetCurrentUserId();
        if (userId != Guid.Empty)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }
        await base.OnConnectedAsync();
    }
}