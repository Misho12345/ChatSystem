using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models;

public class Conversation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public List<Guid> ParticipantIds { get; set; } = [];

    public EmbeddedMessage? LastMessage { get; set; }

    public Dictionary<string, DateTime> LastReadTimestamps { get; set; } = new();

    [BsonIgnore]
    public int UnreadCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class EmbeddedMessage
{
    public required string MessageId { get; set; }


    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }
    public required string Text { get; set; }
    public DateTime Timestamp { get; set; }
}