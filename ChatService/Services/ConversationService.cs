using ChatService.Models;
using MongoDB.Driver;

namespace ChatService.Services;

/// <summary>
/// Service for managing conversations and messages in the chat application.
/// </summary>
public class ConversationService(IMongoDatabase database) : IConversationService
{
    // MongoDB collection for conversations
    private readonly IMongoCollection<Conversation> _conversations =
        database.GetCollection<Conversation>("Conversations");

    // MongoDB collection for messages
    private readonly IMongoCollection<Message> _messages = database.GetCollection<Message>("Messages");

    /// <summary>
    /// Ensures necessary indexes are created in the database for conversations and messages.
    /// </summary>
    public async Task EnsureIndexesAsync()
    {
        // Create indexes for conversations based on participant IDs and updated timestamp
        var conversationIndexKeysDefinition = Builders<Conversation>.IndexKeys
            .Ascending(c => c.ParticipantIds)
            .Descending(c => c.UpdatedAt);
        await _conversations.Indexes.CreateOneAsync(
            new CreateIndexModel<Conversation>(conversationIndexKeysDefinition));

        // Create indexes for messages based on conversation ID and timestamp
        var messageIndexKeysDefinition = Builders<Message>.IndexKeys
            .Ascending(m => m.ConversationId)
            .Descending(m => m.Timestamp);
        await _messages.Indexes.CreateOneAsync(new CreateIndexModel<Message>(messageIndexKeysDefinition));

        // Create index for messages based on sender ID
        var senderIndexDefinition = Builders<Message>.IndexKeys.Ascending(m => m.SenderId);
        await _messages.Indexes.CreateOneAsync(new CreateIndexModel<Message>(senderIndexDefinition));
    }

    /// <summary>
    /// Creates a new conversation between two users or retrieves an existing one.
    /// </summary>
    /// <param name="user1Id">The ID of the first user.</param>
    /// <param name="user2Id">The ID of the second user.</param>
    /// <returns>The conversation object.</returns>
    public async Task<Conversation> CreateOrGetConversationAsync(Guid user1Id, Guid user2Id)
    {
        var participantList = new List<Guid> { user1Id, user2Id }.OrderBy(id => id).ToList();

        // Filter to find a conversation with the specified participants
        var filter = Builders<Conversation>.Filter.All(c => c.ParticipantIds, participantList)
                     & Builders<Conversation>.Filter.Size(c => c.ParticipantIds, 2);

        var conversation = await _conversations.Find(filter).FirstOrDefaultAsync();

        if (conversation != null) return conversation;

        // Create a new conversation if none exists
        conversation = new Conversation
        {
            ParticipantIds = participantList,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _conversations.InsertOneAsync(conversation);

        return conversation;
    }

    /// <summary>
    /// Adds a new message to a conversation and updates the conversation's last message and timestamp.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="senderId">The ID of the sender.</param>
    /// <param name="senderTag">The tag of the sender.</param>
    /// <param name="text">The text of the message.</param>
    /// <param name="messageType">The type of the message (default is "text").</param>
    /// <returns>The added message object.</returns>
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

        // Update the conversation with the last message and timestamp
        var update = Builders<Conversation>.Update
            .Set(c => c.LastMessage, embeddedMessage)
            .Set(c => c.UpdatedAt, message.Timestamp);
        await _conversations.UpdateOneAsync(c => c.Id == conversationId, update);

        return message;
    }

    /// <summary>
    /// Retrieves messages from a conversation, optionally filtered by timestamp.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="requestingUserId">The ID of the user requesting the messages.</param>
    /// <param name="beforeTimestamp">Optional timestamp to retrieve messages before a specific time.</param>
    /// <param name="limit">The maximum number of messages to retrieve.</param>
    /// <returns>A list of messages.</returns>
    public async Task<List<Message>> GetMessagesAsync(string conversationId, Guid requestingUserId,
        DateTime? beforeTimestamp, int limit)
    {
        var conv = await _conversations.Find(c => c.Id == conversationId).FirstOrDefaultAsync();

        // Check if the user has access to the conversation
        if (conv == null || !conv.ParticipantIds.Contains(requestingUserId))
        {
            throw new UnauthorizedAccessException("Access denied.");
        }

        var filter = Builders<Message>.Filter.Eq(m => m.ConversationId, conversationId);

        // Apply timestamp filter if provided
        if (beforeTimestamp.HasValue)
        {
            filter &= Builders<Message>.Filter.Lt(m => m.Timestamp, beforeTimestamp.Value);
        }

        return await _messages.Find(filter)
            .Sort(Builders<Message>.Sort.Descending(m => m.Timestamp))
            .Limit(limit)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a conversation by its ID.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="requestingUserId">The ID of the user requesting the conversation.</param>
    /// <returns>The conversation object.</returns>
    public async Task<Conversation> GetByIdAsync(string conversationId, Guid requestingUserId)
    {
        var conv = await _conversations.Find(c => c.Id == conversationId).FirstOrDefaultAsync();
        if (conv == null || !conv.ParticipantIds.Contains(requestingUserId))
            throw new KeyNotFoundException("Conversation not found or access denied.");
        return conv;
    }

    /// <summary>
    /// Retrieves all conversations for a specific user, including unread message counts.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of conversations.</returns>
    public async Task<List<Conversation>> GetUserConversationsAsync(Guid userId)
    {
        var userIdStr = userId.ToString();
        var filter = Builders<Conversation>.Filter.AnyEq(c => c.ParticipantIds, userId);
        var conversations = await _conversations.Find(filter)
            .Sort(Builders<Conversation>.Sort.Descending(c => c.UpdatedAt))
            .ToListAsync();

        var lastReadTimestamps = conversations.ToDictionary(
            conv => conv.Id!,
            conv => conv.LastReadTimestamps.GetValueOrDefault(userIdStr, DateTime.MinValue)
        );

        if (lastReadTimestamps.Count <= 0) return conversations;

        // Build filters for unread messages
        var orFilters = lastReadTimestamps.Select(kvp =>
            Builders<Message>.Filter.And(
                Builders<Message>.Filter.Eq(m => m.ConversationId, kvp.Key),
                Builders<Message>.Filter.Gt(m => m.Timestamp, kvp.Value),
                Builders<Message>.Filter.Ne(m => m.SenderId, userId)
            )
        );
        var match = Builders<Message>.Filter.Or(orFilters);

        // Aggregate unread message counts
        var unreadCounts = await _messages.Aggregate()
            .Match(match)
            .Group(m => m.ConversationId, g => new { ConversationId = g.Key, UnreadCount = g.Count() })
            .ToListAsync();

        var unreadMap = unreadCounts.ToDictionary(x => x.ConversationId!, x => x.UnreadCount);

        // Update conversations with unread counts
        foreach (var conv in conversations)
        {
            conv.UnreadCount = unreadMap.GetValueOrDefault(conv.Id!, 0);
        }

        return conversations;
    }

    /// <summary>
    /// Marks a conversation as read for a specific user.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="userId">The ID of the user.</param>
    public async Task MarkAsReadAsync(string conversationId, Guid userId)
    {
        var filter = Builders<Conversation>.Filter.Eq(c => c.Id, conversationId);
        var update = Builders<Conversation>.Update.Set($"LastReadTimestamps.{userId.ToString()}", DateTime.UtcNow);
        await _conversations.UpdateOneAsync(filter, update);
    }
}