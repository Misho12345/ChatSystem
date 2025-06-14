using ChatService.Models;
using MongoDB.Driver;

namespace ChatService.Services;

public class ConversationService(IMongoDatabase database) : IConversationService
{
    private readonly IMongoCollection<Conversation> _conversations =
        database.GetCollection<Conversation>("Conversations");

    private readonly IMongoCollection<Message> _messages = database.GetCollection<Message>("Messages");

    public async Task EnsureIndexesAsync()
    {
        var conversationIndexKeysDefinition = Builders<Conversation>.IndexKeys
            .Ascending(c => c.ParticipantIds)
            .Descending(c => c.UpdatedAt);
        await _conversations.Indexes.CreateOneAsync(
            new CreateIndexModel<Conversation>(conversationIndexKeysDefinition));

        var messageIndexKeysDefinition = Builders<Message>.IndexKeys
            .Ascending(m => m.ConversationId)
            .Descending(m => m.Timestamp);
        await _messages.Indexes.CreateOneAsync(new CreateIndexModel<Message>(messageIndexKeysDefinition));

        var senderIndexDefinition = Builders<Message>.IndexKeys.Ascending(m => m.SenderId);
        await _messages.Indexes.CreateOneAsync(new CreateIndexModel<Message>(senderIndexDefinition));
    }


    public async Task<Conversation> CreateOrGetConversationAsync(Guid user1Id, Guid user2Id)
    {
        var participantList = new List<Guid> { user1Id, user2Id }.OrderBy(id => id).ToList();

        var filter = Builders<Conversation>.Filter.All(c => c.ParticipantIds, participantList)
                     & Builders<Conversation>.Filter.Size(c => c.ParticipantIds, 2);

        var conversation = await _conversations.Find(filter).FirstOrDefaultAsync();

        if (conversation != null) return conversation;

        conversation = new Conversation
        {
            ParticipantIds = participantList,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _conversations.InsertOneAsync(conversation);

        return conversation;
    }

    public async Task<Message> AddMessageAsync(string conversationId, Guid senderId, string senderTag, string text,
        string messageType = "text")
    {
        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = senderId,
            SenderTag = senderTag,
            Text = text,
            MessageType = messageType,
            Timestamp = DateTime.UtcNow
        };
        await _messages.InsertOneAsync(message);

        var embeddedMessage = new EmbeddedMessage
        {
            MessageId = message.Id!,
            SenderId = senderId,
            Text = text,
            Timestamp = message.Timestamp
        };

        var update = Builders<Conversation>.Update
            .Set(c => c.LastMessage, embeddedMessage)
            .Set(c => c.UpdatedAt, message.Timestamp);
        await _conversations.UpdateOneAsync(c => c.Id == conversationId, update);

        return message;
    }

    public async Task<List<Message>> GetMessagesAsync(string conversationId, Guid requestingUserId,
        DateTime? beforeTimestamp, int limit)
    {
        var conv = await _conversations.Find(c => c.Id == conversationId).FirstOrDefaultAsync();

        if (conv == null || !conv.ParticipantIds.Contains(requestingUserId))
        {
            throw new UnauthorizedAccessException("Access denied.");
        }

        var filter = Builders<Message>.Filter.Eq(m => m.ConversationId, conversationId);


        if (beforeTimestamp.HasValue)
        {
            filter &= Builders<Message>.Filter.Lte(m => m.Timestamp, beforeTimestamp.Value);
        }

        return await _messages.Find(filter)
            .Sort(Builders<Message>.Sort.Descending(m => m.Timestamp))
            .Limit(limit)
            .ToListAsync();
    }

    public async Task<Conversation> GetByIdAsync(string conversationId, Guid requestingUserId)
    {
        var conv = await _conversations.Find(c => c.Id == conversationId).FirstOrDefaultAsync();
        if (conv == null || !conv.ParticipantIds.Contains(requestingUserId))
            throw new KeyNotFoundException("Conversation not found or access denied.");
        return conv;
    }

    public async Task<List<Conversation>> GetUserConversationsAsync(Guid userId)
    {
        var filter = Builders<Conversation>.Filter.AnyEq(c => c.ParticipantIds, userId);
        return await _conversations.Find(filter)
            .Sort(Builders<Conversation>.Sort.Descending(c => c.UpdatedAt))
            .ToListAsync();
    }
}