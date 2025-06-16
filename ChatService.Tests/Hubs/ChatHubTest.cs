using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ChatService.Hubs;
using ChatService.Models;
using ChatService.Models.DTOs;
using ChatService.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChatService.Tests.Hubs;

/// <summary>
/// Unit tests for the ChatHub class, which handles SignalR communication for chat functionality.
/// </summary>
public class ChatHubTest
{
    private readonly Mock<IConversationService> _mockConversationService;
    private readonly Mock<ILogger<ChatHub>> _mockLogger;
    private readonly Mock<IHubCallerClients> _mockClients;
    private readonly Mock<IClientProxy> _mockClientProxy;
    private readonly Mock<IGroupManager> _mockGroups;
    private readonly ChatHub _chatHub;

    private readonly Guid _testSenderId = Guid.NewGuid();
    private const string TestSenderTag = "testuser#1234";

    /// <summary>
    /// Initializes the test class by setting up mocks and creating an instance of ChatHub.
    /// </summary>
    public ChatHubTest()
    {
        _mockConversationService = new Mock<IConversationService>();
        _mockLogger = new Mock<ILogger<ChatHub>>();
        var mockHubContext = new Mock<HubCallerContext>();
        _mockClients = new Mock<IHubCallerClients>();
        _mockClientProxy = new Mock<IClientProxy>();
        _mockGroups = new Mock<IGroupManager>();

        // Setup mock HubCallerContext with user information and connection details.
        mockHubContext.Setup(c => c.UserIdentifier).Returns(_testSenderId.ToString());
        mockHubContext.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, _testSenderId.ToString()),
            new Claim("tag", TestSenderTag)
        ], "mock")));
        mockHubContext.Setup(c => c.ConnectionId).Returns("test_connection_id");

        // Setup mock clients and groups for SignalR communication.
        _mockClients.Setup(c => c.User(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        _mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        _mockClients.Setup(c => c.OthersInGroup(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        _mockClients.Setup(c => c.Others).Returns(_mockClientProxy.Object);
        _mockClients.Setup(c => c.All).Returns(_mockClientProxy.Object);

        _mockGroups.Setup(g => g.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Create an instance of ChatHub with the mocked dependencies.
        _chatHub = new ChatHub(_mockConversationService.Object, _mockLogger.Object)
        {
            Context = mockHubContext.Object,
            Clients = _mockClients.Object,
            Groups = _mockGroups.Object
        };
    }

    /// <summary>
    /// Tests that SendMessage sends a message to all participants in a valid conversation.
    /// </summary>
    [Fact]
    public async Task SendMessage_WhenConversationAndMessageAreValid_SendsToAllParticipants()
    {
        var conversationId = "conv-1";
        var messageText = "Hello world!";
        var participantId1 = _testSenderId;
        var participantId2 = Guid.NewGuid();

        var message = new Message
        {
            Id = "msg-1",
            ConversationId = conversationId,
            SenderId = participantId1,
            SenderTag = TestSenderTag,
            Text = messageText,
            Timestamp = DateTime.UtcNow,
            MessageType = "text"
        };

        var conversation = new Conversation
        {
            Id = conversationId,
            ParticipantIds = { participantId1, participantId2 },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Setup mock conversation service to return the message and conversation.
        _mockConversationService.Setup(s =>
                s.AddMessageAsync(conversationId, participantId1, TestSenderTag, messageText, "text"))
            .ReturnsAsync(message);
        _mockConversationService.Setup(s => s.GetByIdAsync(conversationId, participantId1))
            .ReturnsAsync(conversation);

        // Call the SendMessage method.
        await _chatHub.SendMessage(conversationId, messageText);

        // Verify that the message was added and retrieved from the conversation service.
        _mockConversationService.Verify(
            s => s.AddMessageAsync(conversationId, participantId1, TestSenderTag, messageText, "text"), Times.Once);
        _mockConversationService.Verify(s => s.GetByIdAsync(conversationId, participantId1), Times.Once);

        // Verify that the message was sent to all participants.
        _mockClients.Verify(clients => clients.User(participantId1.ToString()), Times.Once);
        _mockClients.Verify(clients => clients.User(participantId2.ToString()), Times.Once);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "ReceiveMessage",
                It.Is<object[]>(o => o != null && o.Length == 1 && (o[0] as MessageDto).Text == messageText),
                It.IsAny<CancellationToken>()
            ),
            Times.Exactly(conversation.ParticipantIds.Count)
        );

        // Verify that logs were created for each participant.
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Message {message.Id} sent to User {participantId1} via SignalR")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Message {message.Id} sent to User {participantId2} via SignalR")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }

    /// <summary>
    /// Tests that SendMessage logs an error and does not send a message when the conversation is not found.
    /// </summary>
    [Fact]
    public async Task SendMessage_WhenConversationNotFound_LogsErrorAndDoesNotSend()
    {
        const string conversationId = "non-existent-conv";
        const string messageText = "Test message";

        // Setup mock conversation service to simulate a non-existent conversation.
        _mockConversationService.Setup(s =>
                s.AddMessageAsync(conversationId, _testSenderId, TestSenderTag, messageText, "text"))
            .ReturnsAsync(new Message
                { Id = "msg-1", ConversationId = conversationId, SenderId = _testSenderId, Text = messageText });
        _mockConversationService.Setup(s => s.GetByIdAsync(conversationId, _testSenderId))
            .ReturnsAsync((Conversation)null);

        // Call the SendMessage method.
        await _chatHub.SendMessage(conversationId, messageText);

        // Verify that the message was added but not sent.
        _mockConversationService.Verify(
            s => s.AddMessageAsync(conversationId, _testSenderId, TestSenderTag, messageText, "text"), Times.Once);
        _mockConversationService.Verify(s => s.GetByIdAsync(conversationId, _testSenderId), Times.Once);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()),
            Times.Never
        );

        // Verify that an error log was created.
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Could not find conversation {conversationId} to deliver message msg-1")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }

    /// <summary>
    /// Tests that JoinConversation adds the connection to the specified group.
    /// </summary>
    [Fact]
    public async Task JoinConversation_AddsConnectionToGroup()
    {
        const string conversationId = "group-conv-1";
        const string connectionId = "test_connection_id";

        // Call the JoinConversation method.
        await _chatHub.JoinConversation(conversationId);

        // Verify that the connection was added to the group.
        _mockGroups.Verify(g => g.AddToGroupAsync(connectionId, conversationId, It.IsAny<CancellationToken>()),
            Times.Once);

        // Verify that a log was created for the group join.
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains($"Connection {connectionId} joined group {conversationId}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }

    /// <summary>
    /// Tests that OnConnectedAsync adds the user to their own group based on their user ID.
    /// </summary>
    [Fact]
    public async Task OnConnectedAsync_AddsUserToGroup()
    {
        const string connectionId = "test_connection_id";
        var userId = _testSenderId.ToString();

        // Call the OnConnectedAsync method.
        await _chatHub.OnConnectedAsync();

        // Verify that the user was added to their group.
        _mockGroups.Verify(g => g.AddToGroupAsync(connectionId, userId, It.IsAny<CancellationToken>()), Times.Once);
    }
}