namespace ChatService.Models;

public class Message
{
    public string Id { get; set; }


    public string ConversationId { get; set; }


    public Guid SenderId { get; set; }

    public string Text { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string MessageType { get; set; } = "text"; // For future image/file support
    // public List<Guid> ReadBy { get; set; } = new List<Guid>(); // Optional for read receipts
}