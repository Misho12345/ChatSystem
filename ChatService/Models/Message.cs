using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }


    public string ConversationId { get; set; }


    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }

    public string SenderTag { get; set; }


    public string Text { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string MessageType { get; set; } = "text";
    // public List<Guid> ReadBy { get; set; } = new List<Guid>();
}