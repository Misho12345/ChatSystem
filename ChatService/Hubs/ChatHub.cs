using ChatService.Models.DTOs;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs;

/// <summary>
/// SignalR hub for managing real-time chat functionality.
/// </summary>
[Authorize]
public class ChatHub(IConversationService conversationService, ILogger<ChatHub> logger) : Hub
{
    /// <summary>
    /// Sends a message to all participants in a conversation.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="text">The text content of the message.</param>
    public async Task SendMessage(string conversationId, string text)
    {
        var senderId = Guid.Parse(Context.UserIdentifier!);

        var senderTag = Context.User!.FindFirst("tag")?.Value ?? "Unknown";

        var message = await conversationService.AddMessageAsync(conversationId, senderId, senderTag, text);

        var conversation = await conversationService.GetByIdAsync(conversationId, senderId);
        if (conversation == null)
        {
            logger.LogError("Could not find conversation {ConversationId} to deliver message {MessageId}",
                conversationId, message.Id);
            return;
        }

        var messageDto = new MessageDto(
            message.Id!,
            message.ConversationId!,
            message.SenderId,
            message.SenderTag!,
            message.Text!,
            message.Timestamp,
            message.MessageType
        );

        foreach (var participantId in conversation.ParticipantIds)
        {
            await Clients.User(participantId.ToString()).SendAsync("ReceiveMessage", messageDto);
            logger.LogInformation("Message {MessageId} sent to User {UserId} via SignalR", message.Id, participantId);
        }
    }

    /// <summary>
    /// Adds the current connection to a SignalR group representing a conversation.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation to join.</param>
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        logger.LogInformation("Connection {ConnectionId} joined group {ConversationId}", Context.ConnectionId,
            conversationId);
    }

    /// <summary>
    /// Handles the event when a client connects to the hub.
    /// Adds the connection to the user's SignalR group if authenticated.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            logger.LogInformation(
                "User {UserId} connected with ConnectionId {ConnectionId} and joined their user group.", userId,
                Context.ConnectionId);
        }
        else
        {
            logger.LogWarning("An unauthenticated user connected with ConnectionId {ConnectionId}.",
                Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }
}