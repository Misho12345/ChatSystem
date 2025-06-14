using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatService.Services;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace ChatService.Tests.Services;

public class ConversationServiceTest : IDisposable
{
    private readonly MongoDbRunner _runner;
    private readonly ConversationService _service;

    public ConversationServiceTest()
    {
        _runner = MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        var database = client.GetDatabase("TestDb");
        _service = new ConversationService(database);
    }

    public void Dispose()
    {
        _runner.Dispose();
    }

    [Fact]
    public async Task EnsureIndexesAsync_DoesNotThrow()
    {
        await _service.EnsureIndexesAsync();
    }

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

    [Fact]
    public async Task AddMessageAsync_InsertsMessageAndUpdatesConversation()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);

        var msg = await _service.AddMessageAsync(conv.Id, u1, "tag", "hello", "text");
        Assert.NotNull(msg.Id);
        Assert.Equal(conv.Id, msg.ConversationId);

        var updated = await _service.GetByIdAsync(conv.Id, u1);
        Assert.NotNull(updated.LastMessage);
        Assert.Equal(msg.Id, updated.LastMessage.MessageId);
        Assert.Equal("hello", updated.LastMessage.Text);
    }

    [Fact]
    public async Task GetMessagesAsync_ReturnsDescendingAndHonorsLimitAndTimestamp()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);

        var m1 = await _service.AddMessageAsync(conv.Id, u1, "t", "first");
        await Task.Delay(10);
        var m2 = await _service.AddMessageAsync(conv.Id, u1, "t", "second");
        await Task.Delay(10);
        var m3 = await _service.AddMessageAsync(conv.Id, u1, "t", "third");

        var all = await _service.GetMessagesAsync(conv.Id, u1, null, 10);
        Assert.Equal(3, all.Count);
        Assert.Equal("third", all[0].Text);

        var before = all[1].Timestamp;
        var limited = await _service.GetMessagesAsync(conv.Id, u1, before, 1);
        Assert.Single(limited);
        Assert.Equal("second", limited[0].Text);
    }

    [Fact]
    public async Task GetMessagesAsync_ThrowsWhenUnauthorized()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var u3 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.GetMessagesAsync(conv.Id, u3, null, 10));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAndThrowsOnUnauthorized()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var u3 = Guid.NewGuid();
        var conv = await _service.CreateOrGetConversationAsync(u1, u2);

        var fetched = await _service.GetByIdAsync(conv.Id, u1);
        Assert.Equal(conv.Id, fetched.Id);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetByIdAsync(conv.Id, u3));
    }

    [Fact]
    public async Task GetUserConversationsAsync_FiltersAndSorts()
    {
        var u1 = Guid.NewGuid();
        var u2 = Guid.NewGuid();
        var c1 = await _service.CreateOrGetConversationAsync(u1, u2);
        await Task.Delay(10);
        var c2 = await _service.CreateOrGetConversationAsync(u1, Guid.NewGuid());

        var list = await _service.GetUserConversationsAsync(u1);
        Assert.Equal(2, list.Count);
        Assert.Equal(c2.Id, list.First().Id);
    }
}