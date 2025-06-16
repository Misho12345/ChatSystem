using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models;

/// <summary>
/// Represents a message in a chat conversation.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the unique identifier for the message.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the conversation to which the message belongs.
    /// </summary>
    public string? ConversationId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sender of the message.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }

    /// <summary>
    /// Gets or sets the tag of the sender (e.g., username or display name).
    /// </summary>
    public string? SenderTag { get; set; }

    /// <summary>
    /// Gets or sets the text content of the message.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when the message was created.
    /// Defaults to the current UTC time.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the type of the message (e.g., "text", "image").
    /// Defaults to "text".
    /// </summary>
    public string MessageType { get; set; } = "text";
}