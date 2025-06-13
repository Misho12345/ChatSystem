namespace ChatService.Models.DTOs;

public class ConversationDto(
    string id,
    List<Guid> participantIds,
    LastMessageDto? lastMessage,
    DateTime createdAt,
    DateTime updatedAt)
{
    public string Id { get; set; } = id;
    public List<Guid> ParticipantIds { get; set; } = participantIds;
    public LastMessageDto? LastMessage { get; set; } = lastMessage;
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
}