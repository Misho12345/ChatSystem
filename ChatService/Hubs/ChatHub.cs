using ChatService.Models.DTOs;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs;

[Authorize]
public class ChatHub(IConversationService conversationService, ILogger<ChatHub> logger)
    : Hub
{
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

    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        logger.LogInformation("Connection {ConnectionId} joined group {ConversationId}", Context.ConnectionId,
            conversationId);
    }
}