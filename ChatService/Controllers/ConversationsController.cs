using ChatService.Models.DTOs;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConversationsController(IConversationService conversationService, ILogger<ConversationsController> logger)
    : ControllerBase
{
    private Guid GetUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdString, out var userId))
        {
            return userId;
        }

        logger.LogError("User ID not found in token claims.");
        throw new InvalidOperationException("User ID not found in token.");
    }

    private string GetUserTag() 
    {
        return User.FindFirstValue("tag") ?? "UnknownUser";
    }


    [HttpGet] // GET /api/conversations
    public async Task<IActionResult> GetUserConversations()
    {
        var userId = GetUserId();
        logger.LogInformation("Fetching conversations for User ID: {UserId}", userId);

        var conversations = await conversationService.GetUserConversationsAsync(userId);

        var conversationDtos = conversations.Select(c => new ConversationDto(
            c.Id,
            c.ParticipantIds,
            c.LastMessage != null
                ? new LastMessageDto(
                    c.LastMessage.MessageId,
                    c.LastMessage.SenderId,
                    "Tag",
                    c.LastMessage.Text,
                    c.LastMessage.Timestamp)
                : null,
            c.CreatedAt,
            c.UpdatedAt
        )).ToList();

        logger.LogInformation("Returning {Count} conversations for User ID: {UserId}", conversationDtos.Count, userId);
        return Ok(conversationDtos);
    }

    [HttpGet("{conversationId}/messages")] // GET /api/conversations/{conversationId}/messages?beforeTimestamp=...&limit=...
    public async Task<IActionResult> GetMessages(string conversationId, [FromQuery] DateTime? beforeTimestamp,
        [FromQuery] int limit = 20)
    {
        var userId = GetUserId();
        logger.LogInformation(
            "Fetching messages for Conversation ID: {ConversationId}, User ID: {UserId}, Before: {BeforeTimestamp}, Limit: {Limit}",
            conversationId, userId, beforeTimestamp, limit);

        // TODO: Add a check in IConversationService.GetMessagesAsync to ensure 'userId' is a participant of 'conversationId' for security.
        var messages = await conversationService.GetMessagesAsync(conversationId, beforeTimestamp, limit);

        // Map to DTOs. MessageDto requires SenderTag.
        // This is a common challenge in microservices. The ChatService might not know the tag.
        // Options:
        // 1. Client resolves SenderId to SenderTag using its friend list or by querying UserAccountService.
        // 2. ChatService calls UserAccountService (can add latency, complexity).
        // 3. UserAccountService publishes user updates (e.g., tag changes) via events, and ChatService caches/stores relevant user info.
        // For this MVP, we'll assume the MessageDto constructor in ChatService might get the tag from claims when message is created,
        // or the client is responsible for mapping SenderId to a display name/tag.
        // The DTO here will just pass what the service provides.
        var messageDtos = messages.Select(m => new MessageDto(
            m.Id,
            m.ConversationId,
            m.SenderId,
            "Tag", // Placeholder: This tag should ideally come from a lookup or be passed by the client/hub
            m.Text,
            m.Timestamp,
            m.MessageType
        )).OrderBy(m => m.Timestamp).ToList();

        logger.LogInformation("Returning {Count} messages for Conversation ID: {ConversationId}", messageDtos.Count,
            conversationId);
        return Ok(messageDtos);
    }

    [HttpPost("initiate")] // POST /api/conversations/initiate
    public async Task<IActionResult> InitiateConversation(InitiateConversationRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var initiatorId = GetUserId();
        logger.LogInformation(
            "User ID: {InitiatorId} attempting to initiate conversation with Recipient ID: {RecipientId}", initiatorId,
            request.RecipientId);

        if (initiatorId == request.RecipientId)
        {
            logger.LogWarning("User ID: {InitiatorId} attempted to initiate conversation with self.", initiatorId);
            return BadRequest(new { message = "Cannot initiate a conversation with yourself." });
        }

        var conversation = await conversationService.CreateOrGetConversationAsync(initiatorId, request.RecipientId);

        if (conversation == null)
        {
            logger.LogError("Failed to create or get conversation between {InitiatorId} and {RecipientId}",
                initiatorId, request.RecipientId);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Could not initiate conversation." });
        }

        var conversationDto = new ConversationDto(
            conversation.Id,
            conversation.ParticipantIds,
            conversation.LastMessage != null
                ? new LastMessageDto(
                    conversation.LastMessage.MessageId,
                    conversation.LastMessage.SenderId,
                    "Tag", // Placeholder for LastMessage.SenderTag
                    conversation.LastMessage.Text,
                    conversation.LastMessage.Timestamp)
                : null,
            conversation.CreatedAt,
            conversation.UpdatedAt
        );

        logger.LogInformation(
            "Conversation initiated/retrieved: {ConversationId} for User ID: {InitiatorId} and Recipient ID: {RecipientId}",
            conversation.Id, initiatorId, request.RecipientId);
        return Ok(conversationDto);
    }
}