namespace ChatService.Models.DTOs;

public class LastMessageDto(string messageId, Guid senderId, string senderTag, string text, DateTime timestamp)
{
    public string MessageId { get; set; } = messageId;
    public Guid SenderId { get; set; } = senderId;
    public string SenderTag { get; set; } = senderTag;
    public string Text { get; set; } = text;
    public DateTime Timestamp { get; set; } = timestamp;
}