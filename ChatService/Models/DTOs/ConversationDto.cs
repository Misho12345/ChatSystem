namespace ChatService.Models.DTOs;

/// <summary>
/// Represents a conversation data transfer object (DTO).
/// </summary>
/// <param name="id">The unique identifier of the conversation.</param>
/// <param name="participantIds">The list of participant IDs in the conversation.</param>
/// <param name="lastMessage">The last message in the conversation, if available.</param>
/// <param name="createdAt">The timestamp when the conversation was created.</param>
/// <param name="updatedAt">The timestamp when the conversation was last updated.</param>
/// <param name="unreadCount">The number of unread messages in the conversation.</param>
public class ConversationDto(
    string id,
    List<Guid> participantIds,
    LastMessageDto? lastMessage,
    DateTime createdAt,
    DateTime updatedAt,
    int unreadCount)
{
    /// <summary>
    /// Gets or sets the unique identifier of the conversation.
    /// </summary>
    public string Id { get; set; } = id;

    /// <summary>
    /// Gets or sets the list of participant IDs in the conversation.
    /// </summary>
    public List<Guid> ParticipantIds { get; set; } = participantIds;

    /// <summary>
    /// Gets or sets the last message in the conversation, if available.
    /// </summary>
    public LastMessageDto? LastMessage { get; set; } = lastMessage;

    /// <summary>
    /// Gets or sets the timestamp when the conversation was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = createdAt;

    /// <summary>
    /// Gets or sets the timestamp when the conversation was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = updatedAt;

    /// <summary>
    /// Gets or sets the number of unread messages in the conversation.
    /// </summary>
    public int UnreadCount { get; set; } = unreadCount;
}