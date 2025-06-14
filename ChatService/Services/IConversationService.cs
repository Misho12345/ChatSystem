using ChatService.Models;

namespace ChatService.Services;

public interface IConversationService
{
    Task<Conversation> CreateOrGetConversationAsync(Guid user1Id, Guid user2Id);

    Task<Message> AddMessageAsync(string conversationId, Guid senderId, string senderTag, string text,
        string messageType = "text");

    Task<List<Message>> GetMessagesAsync(string conversationId, Guid requestingUserId, DateTime? beforeTimestamp,
        int limit);
    
    Task<Conversation> GetByIdAsync(string conversationId, Guid requestingUserId);

    Task<List<Conversation>> GetUserConversationsAsync(Guid userId);
    Task EnsureIndexesAsync();
}