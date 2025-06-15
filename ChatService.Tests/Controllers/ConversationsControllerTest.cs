using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatService.Controllers;
using ChatService.Models;
using ChatService.Models.DTOs;
using ChatService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChatService.Tests.Controllers;

public class ConversationsControllerTest
{
    private readonly Mock<IConversationService> _mockConversationService;
    private readonly ConversationsController _controller;
    private readonly Guid _testUserId = Guid.NewGuid();

    public ConversationsControllerTest()
    {
        _mockConversationService = new Mock<IConversationService>();
        var mockLogger = new Mock<ILogger<ConversationsController>>();

        _controller = new ConversationsController(_mockConversationService.Object, mockLogger.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()),
            new Claim("tag", "testuser#1234")
        ], "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task GetMessages_WhenConversationExists_ReturnsOkWithMessages()
    {
        const string conversationId = "conv-1";
        var messages = new List<Message>
        {
            new()
            {
                Id = "msg-1", ConversationId = conversationId, Text = "Hello", SenderId = _testUserId,
                Timestamp = DateTime.UtcNow
            },
            new()
            {
                Id = "msg-2", ConversationId = conversationId, Text = "Hi", SenderId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow.AddMinutes(-1)
            }
        };
        _mockConversationService.Setup(s => s.GetMessagesAsync(conversationId, _testUserId, null, 20))
            .ReturnsAsync(messages);

        var result = await _controller.GetMessages(conversationId, null, 20);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDtos = Assert.IsType<IEnumerable<MessageDto>>(okResult.Value, exactMatch: false);
        var messageDtos = returnedDtos as MessageDto[] ?? returnedDtos.ToArray();
        Assert.Equal(2, messageDtos.Length);
        Assert.Equal("Hello", messageDtos.First().Text);
    }

    [Fact]
    public async Task GetMessages_WhenServiceThrowsUnauthorized_ShouldPropagateException()
    {
        const string conversationId = "conv-1";

        _mockConversationService.Setup(s => s.GetMessagesAsync(It.IsAny<string>(), It.IsAny<Guid>(), null, 20))
            .ThrowsAsync(new UnauthorizedAccessException());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.GetMessages(conversationId, null, 20));
    }

    [Fact]
    public async Task InitiateConversation_WithValidRecipient_ReturnsOkWithConversationDto()
    {
        var recipientId = Guid.NewGuid();
        var request = new InitiateConversationRequestDto { RecipientId = recipientId };
        var conversation = new Conversation
        {
            Id = "new-conv", ParticipantIds = [_testUserId, recipientId], CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _mockConversationService.Setup(s => s.CreateOrGetConversationAsync(_testUserId, recipientId))
            .ReturnsAsync(conversation);

        var result = await _controller.InitiateConversation(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<ConversationDto>(okResult.Value);
        Assert.Equal(conversation.Id, returnedDto.Id);
        Assert.Contains(recipientId, returnedDto.ParticipantIds);
        _mockConversationService.Verify(s => s.CreateOrGetConversationAsync(_testUserId, recipientId), Times.Once);
    }

    [Fact]
    public async Task InitiateConversation_WithSelfAsRecipient_ReturnsBadRequest()
    {
        var request = new InitiateConversationRequestDto { RecipientId = _testUserId };

        var result = await _controller.InitiateConversation(request);

        Assert.IsType<BadRequestObjectResult>(result);

        _mockConversationService.Verify(s => s.CreateOrGetConversationAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task InitiateConversation_WhenServiceReturnsNull_ReturnsInternalServerError()
    {
        var recipientId = Guid.NewGuid();
        var request = new InitiateConversationRequestDto { RecipientId = recipientId };
        _mockConversationService.Setup(s => s.CreateOrGetConversationAsync(_testUserId, recipientId))
            .ReturnsAsync((Conversation)null);

        var result = await _controller.InitiateConversation(request);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetConversations_WhenUserHasConversations_ReturnsOkWithListOfConversationDtos()
    {
        var conversations = new List<Conversation>
        {
            new()
            {
                Id = "conv-1", ParticipantIds = { _testUserId, Guid.NewGuid() }, UnreadCount = 2,
                LastMessage = new EmbeddedMessage
                    { MessageId = "m1", SenderId = Guid.NewGuid(), Text = "Hi", Timestamp = DateTime.UtcNow },
                CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = "conv-2", ParticipantIds = { _testUserId, Guid.NewGuid() }, UnreadCount = 0,
                CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            }
        };
        _mockConversationService.Setup(s => s.GetUserConversationsAsync(_testUserId)).ReturnsAsync(conversations);

        var result = await _controller.GetConversations();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDtos = Assert.IsType<IEnumerable<ConversationDto>>(okResult.Value, exactMatch: false);
        var conversationDtos = returnedDtos as ConversationDto[] ?? returnedDtos.ToArray();
        Assert.Equal(2, conversationDtos.Length);
        Assert.Equal(2, conversationDtos.First().UnreadCount);
        Assert.NotNull(conversationDtos.First().LastMessage);
        Assert.Equal("Hi", conversationDtos.First().LastMessage!.Text);
        Assert.Null(conversationDtos.Last().LastMessage);
    }

    [Fact]
    public async Task MarkConversationAsRead_WithValidRequest_CallsServiceAndReturnsNoContent()
    {
        const string conversationId = "test-conversation-123";
        _mockConversationService
            .Setup(s => s.MarkAsReadAsync(It.IsAny<string>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.MarkConversationAsRead(conversationId);

        _mockConversationService.Verify(
            s => s.MarkAsReadAsync(conversationId, _testUserId),
            Times.Once);

        Assert.IsType<NoContentResult>(result);
    }
}