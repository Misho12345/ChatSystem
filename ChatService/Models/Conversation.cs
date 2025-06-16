using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models;

/// <summary>
/// Represents a conversation between participants in the chat service.
/// </summary>
public class Conversation
{
    /// <summary>
    /// Gets or sets the unique identifier for the conversation.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the list of participant IDs in the conversation.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public List<Guid> ParticipantIds { get; set; } = [];

    /// <summary>
    /// Gets or sets the last message in the conversation.
    /// </summary>
    public EmbeddedMessage? LastMessage { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of last read timestamps for each participant.
    /// The key is the participant's ID as a string, and the value is the timestamp.
    /// </summary>
    public Dictionary<string, DateTime> LastReadTimestamps { get; set; } = new();

    /// <summary>
    /// Gets or sets the count of unread messages in the conversation.
    /// This property is ignored by MongoDB.
    /// </summary>
    [BsonIgnore]
    public int UnreadCount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when the conversation was created.
    /// Defaults to the current UTC time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the timestamp indicating when the conversation was last updated.
    /// Defaults to the current UTC time.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Represents an embedded message within a conversation.
/// </summary>
public class EmbeddedMessage
{
    /// <summary>
    /// Gets or sets the unique identifier for the message.
    /// </summary>
    public required string MessageId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the sender of the message.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }

    /// <summary>
    /// Gets or sets the text content of the message.
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when the message was created.
    /// </summary>
    public DateTime Timestamp { get; set; }
}