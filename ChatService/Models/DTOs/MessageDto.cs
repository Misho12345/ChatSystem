namespace ChatService.Models.DTOs;

public class MessageDto(
    string id,
    string conversationId,
    Guid senderId,
    string senderTag,
    string text,
    DateTime timestamp,
    string messageType)
{
    public string Id { get; set; } = id;
    public string ConversationId { get; set; } = conversationId;
    public Guid SenderId { get; set; } = senderId;
    public string SenderTag { get; set; } = senderTag;
    public string Text { get; set; } = text;
    public DateTime Timestamp { get; set; } = timestamp;
    public string MessageType { get; set; } = messageType ?? "text";
}