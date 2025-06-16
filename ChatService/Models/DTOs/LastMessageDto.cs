namespace ChatService.Models.DTOs;

/// <summary>
/// Represents the last message in a conversation.
/// </summary>
/// <param name="messageId">The unique identifier of the message.</param>
/// <param name="senderId">The unique identifier of the sender.</param>
/// <param name="senderTag">The tag or username of the sender.</param>
/// <param name="text">The content of the message.</param>
/// <param name="timestamp">The timestamp when the message was sent.</param>
public class LastMessageDto(string messageId, Guid senderId, string senderTag, string text, DateTime timestamp)
{
    /// <summary>
    /// Gets or sets the unique identifier of the message.
    /// </summary>
    public string MessageId { get; set; } = messageId;

    /// <summary>
    /// Gets or sets the unique identifier of the sender.
    /// </summary>
    public Guid SenderId { get; set; } = senderId;

    /// <summary>
    /// Gets or sets the tag or username of the sender.
    /// </summary>
    public string SenderTag { get; set; } = senderTag;

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    public string Text { get; set; } = text;

    /// <summary>
    /// Gets or sets the timestamp when the message was sent.
    /// </summary>
    public DateTime Timestamp { get; set; } = timestamp;
}