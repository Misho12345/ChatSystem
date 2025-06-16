namespace ChatService.Models.DTOs;

/// <summary>
/// Represents a request to initiate a conversation.
/// </summary>
public class InitiateConversationRequestDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the recipient with whom the conversation is to be initiated.
    /// </summary>
    public Guid RecipientId { get; set; }
}