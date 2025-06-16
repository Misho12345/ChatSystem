namespace ChatService.Models.DTOs;

/// <summary>
/// Represents a message data transfer object (DTO).
/// </summary>
/// <param name="id">The unique identifier of the message.</param>
/// <param name="conversationId">The unique identifier of the conversation the message belongs to.</param>
/// <param name="senderId">The unique identifier of the sender of the message.</param>
/// <param name="senderTag">The tag or username of the sender.</param>
/// <param name="text">The content of the message.</param>
/// <param name="timestamp">The timestamp when the message was sent.</param>
/// <param name="messageType">The type of the message (e.g., text, image, etc.). Defaults to "text" if not specified.</param>
public class MessageDto(
    string id,
    string conversationId,
    Guid senderId,
    string senderTag,
    string text,
    DateTime timestamp,
    string messageType)
{
    /// <summary>
    /// Gets or sets the unique identifier of the message.
    /// </summary>
    public string Id { get; set; } = id;

    /// <summary>
    /// Gets or sets the unique identifier of the conversation the message belongs to.
    /// </summary>
    public string ConversationId { get; set; } = conversationId;

    /// <summary>
    /// Gets or sets the unique identifier of the sender of the message.
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

    /// <summary>
    /// Gets or sets the type of the message (e.g., text, image, etc.). Defaults to "text" if not specified.
    /// </summary>
    public string MessageType { get; set; } = messageType ?? "text";
}