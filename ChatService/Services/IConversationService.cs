using ChatService.Models;

namespace ChatService.Services;

/// <summary>
/// Interface for managing conversations and messages in the chat service.
/// </summary>
public interface IConversationService
{
    /// <summary>
    /// Creates a new conversation between two users or retrieves an existing one.
    /// </summary>
    /// <param name="user1Id">The ID of the first user.</param>
    /// <param name="user2Id">The ID of the second user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the conversation.</returns>
    Task<Conversation> CreateOrGetConversationAsync(Guid user1Id, Guid user2Id);

    /// <summary>
    /// Adds a new message to a conversation.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="senderId">The ID of the sender.</param>
    /// <param name="senderTag">The tag of the sender.</param>
    /// <param name="text">The text of the message.</param>
    /// <param name="messageType">The type of the message (default is "text").</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added message.</returns>
    Task<Message> AddMessageAsync(string conversationId, Guid senderId, string senderTag, string text,
        string messageType = "text");

    /// <summary>
    /// Retrieves messages from a conversation.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="requestingUserId">The ID of the user requesting the messages.</param>
    /// <param name="beforeTimestamp">Optional timestamp to retrieve messages before a specific time.</param>
    /// <param name="limit">The maximum number of messages to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of messages.</returns>
    Task<List<Message>> GetMessagesAsync(string conversationId, Guid requestingUserId, DateTime? beforeTimestamp,
        int limit);

    /// <summary>
    /// Retrieves a conversation by its ID.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="requestingUserId">The ID of the user requesting the conversation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the conversation.</returns>
    Task<Conversation> GetByIdAsync(string conversationId, Guid requestingUserId);

    /// <summary>
    /// Retrieves all conversations for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of conversations.</returns>
    Task<List<Conversation>> GetUserConversationsAsync(Guid userId);

    /// <summary>
    /// Ensures that necessary indexes are created in the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task EnsureIndexesAsync();

    /// <summary>
    /// Marks a conversation as read for a specific user.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task MarkAsReadAsync(string conversationId, Guid userId);
}