using ChatService.Models;

namespace ChatService.Services;

public interface IConversationService
{
    Task<Conversation> CreateOrGetConversationAsync(Guid user1Id, Guid user2Id);
    Task<Message> AddMessageAsync(string conversationId, Guid senderId, string text, string messageType = "text");
    Task<List<Message>> GetMessagesAsync(string conversationId, DateTime? beforeTimestamp, int limit);
    Task<List<Conversation>> GetUserConversationsAsync(Guid userId);
    Task EnsureIndexesAsync();
}