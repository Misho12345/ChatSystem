using ChatService.Models.DTOs;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatService.Hubs;

[Authorize]
public class ChatHub(IConversationService conversationService, ILogger<ChatHub> logger) : Hub
{
    private Guid GetUserId()
    {
        var userIdString = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }

        throw new InvalidOperationException("User ID not found in token.");
    }

    private string GetUserTag()
    {
        return Context.User?.FindFirstValue("tag") ?? "UnknownUser";
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        logger.LogInformation("User {UserId} connected: {ContextConnectionId}", userId, Context.ConnectionId);
        // Optionally, add user to groups representing their ongoing conversations
        // var conversations = await _conversationService.GetUserConversationsAsync(userId);
        // foreach (var conv in conversations)
        // {
        //     await Groups.AddToGroupAsync(Context.ConnectionId, conv.Id);
        // }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        logger.LogInformation("User {UserId} disconnected: {ContextConnectionId}. Exception: {ExceptionMessage}",
            userId, Context.ConnectionId, exception?.Message);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string conversationId, string messageText)
    {
        var senderId = GetUserId();
        var senderTag = GetUserTag();

        if (string.IsNullOrWhiteSpace(messageText)) return;

        var message = await conversationService.AddMessageAsync(conversationId, senderId, messageText);

        var messageDto = new MessageDto(message.Id, message.ConversationId, message.SenderId, senderTag, message.Text,
            message.Timestamp, message.MessageType);

        await Clients.Group(conversationId).SendAsync("ReceiveMessage", messageDto);
        // Or, if not using groups for 1-on-1, find other participant and send to them by UserId
        // var conversation = await _conversationService.GetConversationByIdAsync(conversationId);
        // var otherParticipantId = conversation.ParticipantIds.FirstOrDefault(id => id!= senderId);
        // if(otherParticipantId!= default(Guid))
        // {
        //    await Clients.User(otherParticipantId.ToString()).SendAsync("ReceiveMessage", messageDto);
        //    await Clients.Caller.SendAsync("ReceiveMessage", messageDto); // Send to self too
        // }
    }

    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        logger.LogInformation("User {Guid} joined group {ConversationId}", GetUserId(), conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        logger.LogInformation("User {Guid} left group {ConversationId}", GetUserId(), conversationId);
    }
}