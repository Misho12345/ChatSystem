namespace ChatService.Models;

public class Conversation
{
    public string Id { get; set; }

    public List<Guid> ParticipantIds { get; set; } = [];

    public EmbeddedMessage? LastMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class EmbeddedMessage
{
    public string MessageId { get; set; }


    public Guid SenderId { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}