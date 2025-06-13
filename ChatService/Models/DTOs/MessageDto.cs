namespace ChatService.Models.DTOs;

public class MessageDto
{
    public string Id { get; set; }
    public string ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public string SenderTag { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
    public string MessageType { get; set; }

    public MessageDto(string id, string conversationId, Guid senderId, string senderTag, string text,
        DateTime timestamp, string messageType)
    {
        Id = id;
        ConversationId = conversationId;
        SenderId = senderId;
        SenderTag = senderTag;
        Text = text;
        Timestamp = timestamp;
        MessageType = messageType ?? "text";
    }

    public MessageDto()
    {
    }
}