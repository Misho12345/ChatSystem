using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Services;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace ChatService.Tests.Services;

/// <summary>
/// Unit tests for the ConversationService class, which handles database operations for conversations and messages.
/// </summary>
public class ConversationServiceTest : IDisposable
{
    private readonly ConversationService _service;
    private readonly MongoDbRunner _runner;

    /// <summary>
    /// Initializes the test class by setting up a MongoDB instance and creating a ConversationService instance.
    /// </summary>
    public ConversationServiceTest()
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            _runner = MongoDbRunner.Start();
            connectionString = _runner.ConnectionString;
        }

        var client = new MongoClient(connectionString);

        var dbName = $"TestDb_{Guid.NewGuid()}";
        var database = client.GetDatabase(dbName);

        _service = new ConversationService(database);
    }

    /// <summary>
    /// Disposes of the MongoDB runner instance if it was created.
    /// </summary>
    public void Dispose()
    {
        _runner?.Dispose();
    }

    /// <summary>
    /// Tests that EnsureIndexesAsync does not throw any exceptions.
    /// </summary>
    [Fact]
    public async Task EnsureIndexesAsync_DoesNotThrow()
    {
        await _service.EnsureIndexesAsync();
    }

    /// <summary>
    /// Tests that CreateOrGetConversationAsync creates a new conversation or retrieves an existing one.
    /// </summary>
    [Fact]
    public async Task CreateOrGetConversationAsync_CreatesAndReturnsSame()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();

        var conv1 = await _service.CreateOrGetConversationAsync(u1, u2);
        Assert.NotNull(conv1);
        Assert.Equal(2, conv1.ParticipantIds.Count);
        Assert.Contains(u1, conv1.ParticipantIds);
        Assert.Contains(u2, conv1.ParticipantIds);

        var conv2 = await _service.CreateOrGetConversationAsync(u1, u2);
        Assert.Equal(conv1.Id, conv2.Id);
    }

    /// <summary>
    /// Tests that AddMessageAsync inserts a message and updates the conversation's last message.
    /// </summary>
    [Fact]
    public async Task AddMessageAsync_InsertsMessageAndUpdatesConversation()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);

        var msg = await _service.AddMessageAsync(conv.Id!, u1, "tag", "hello");
        Assert.NotNull(msg.Id);
        Assert.Equal(conv.Id, msg.ConversationId);

        var updated = await _service.GetByIdAsync(conv.Id, u1);
        Assert.NotNull(updated.LastMessage);
        Assert.Equal(msg.Id, updated.LastMessage.MessageId);
        Assert.Equal("hello", updated.LastMessage.Text);
    }

    /// <summary>
    /// Tests that GetMessagesAsync returns messages in descending order and respects limit and timestamp filters.
    /// </summary>
    [Fact]
    public async Task GetMessagesAsync_ReturnsDescendingAndHonorsLimitAndTimestamp()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);

        await _service.AddMessageAsync(conv.Id!, u1, "t", "first");
        await Task.Delay(10);
        await _service.AddMessageAsync(conv.Id, u1, "t", "second");
        await Task.Delay(10);
        await _service.AddMessageAsync(conv.Id, u1, "t", "third");

        var all = await _service.GetMessagesAsync(conv.Id, u1, null, 10);
        Assert.Equal(3, all.Count);
        Assert.Equal("third", all[0].Text);

        var before = all[0].Timestamp;
        var limited = await _service.GetMessagesAsync(conv.Id, u1, before, 1);
        Assert.Single(limited);
        Assert.Equal("second", limited[0].Text);
    }

    /// <summary>
    /// Tests that GetMessagesAsync throws an UnauthorizedAccessException when the user is not a participant.
    /// </summary>
    [Fact]
    public async Task GetMessagesAsync_ThrowsWhenUnauthorized()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var u3 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.GetMessagesAsync(conv.Id!, u3, null, 10));
    }

    /// <summary>
    /// Tests that GetByIdAsync returns the conversation for authorized users and throws for unauthorized users.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ReturnsAndThrowsOnUnauthorized()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var u3 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);

        var fetched = await _service.GetByIdAsync(conv.Id!, u1);
        Assert.Equal(conv.Id, fetched.Id);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(conv.Id, u3));
    }

    /// <summary>
    /// Tests that GetUserConversationsAsync filters conversations by user and sorts them by last message timestamp.
    /// </summary>
    [Fact]
    public async Task GetUserConversationsAsync_FiltersAndSorts()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        await _service.CreateOrGetConversationAsync(u1, u2);
        await Task.Delay(10);
        var c2 = await _service.CreateOrGetConversationAsync(u1, Guid.NewGuid());
        await _service.AddMessageAsync(c2.Id!, u1, "t", "latest message");

        var list = await _service.GetUserConversationsAsync(u1);
        Assert.Equal(2, list.Count);
        Assert.Equal(c2.Id, list.First().Id);
    }
}