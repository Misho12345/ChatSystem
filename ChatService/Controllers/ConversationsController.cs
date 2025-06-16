using ChatService.Models.DTOs;
using ChatService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatService.Controllers;

/// <summary>
/// Controller for managing conversations and related operations.
/// Requires authorization for all endpoints.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConversationsController(IConversationService conversationService, ILogger<ConversationsController> logger)
    : ControllerBase
{
    /// <summary>
    /// Retrieves the user ID from the token claims.
    /// </summary>
    /// <returns>The user ID as a <see cref="Guid"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the user ID is not found in the token claims.</exception>
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

    /// <summary>
    /// Retrieves the user tag from the token claims.
    /// </summary>
    /// <returns>The user tag as a <see cref="string"/>. Defaults to "UnknownUser" if not found.</returns>
    private string GetUserTag()
    {
        return User.FindFirstValue("tag") ?? "UnknownUser";
    }

    /// <summary>
    /// Retrieves messages for a specific conversation.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation.</param>
    /// <param name="beforeTimestamp">Optional timestamp to fetch messages before.</param>
    /// <param name="limit">The maximum number of messages to retrieve. Defaults to 20.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the list of messages in the conversation.
    /// </returns>
    [HttpGet("{conversationId}/messages")]
    public async Task<IActionResult> GetMessages(string conversationId, [FromQuery] DateTime? beforeTimestamp,
        [FromQuery] int limit = 20)
    {
        var userId = GetUserId();
        logger.LogInformation(
            "Fetching messages for Conversation ID: {ConversationId}, User ID: {UserId}, Before: {BeforeTimestamp}, Limit: {Limit}",
            conversationId, userId, beforeTimestamp, limit);

        var messages = await conversationService.GetMessagesAsync(conversationId, userId, beforeTimestamp, limit);

        var messageDtos = messages.Select(m => new MessageDto(
            m.Id!,
            m.ConversationId!,
            m.SenderId,
            "Tag",
            m.Text!,
            m.Timestamp,
            m.MessageType
        )).OrderByDescending(m => m.Timestamp).ToList();

        logger.LogInformation("Returning {Count} messages for Conversation ID: {ConversationId}", messageDtos.Count,
            conversationId);
        return Ok(messageDtos);
    }

    /// <summary>
    /// Initiates a new conversation or retrieves an existing one.
    /// </summary>
    /// <param name="request">The request containing the recipient ID.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the conversation details or an error response.
    /// </returns>
    [HttpPost("initiate")]
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
            conversation.Id!,
            conversation.ParticipantIds,
            conversation.LastMessage != null
                ? new LastMessageDto(
                    conversation.LastMessage.MessageId,
                    conversation.LastMessage.SenderId,
                    conversation.LastMessage.SenderId.ToString(),
                    conversation.LastMessage.Text,
                    conversation.LastMessage.Timestamp)
                : null,
            conversation.CreatedAt,
            conversation.UpdatedAt,
            0
        );

        logger.LogInformation(
            "Conversation initiated/retrieved: {ConversationId} for User ID: {InitiatorId} and Recipient ID: {RecipientId}",
            conversation.Id, initiatorId, request.RecipientId);
        return Ok(conversationDto);
    }

    /// <summary>
    /// Retrieves all conversations for the currently authenticated user.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the list of conversations.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetConversations()
    {
        var userId = GetUserId();
        var conversations = await conversationService.GetUserConversationsAsync(userId);
        var dtos = conversations
            .Select(c => new ConversationDto(
                c.Id!,
                c.ParticipantIds,
                c.LastMessage != null
                    ? new LastMessageDto(
                        c.LastMessage.MessageId,
                        c.LastMessage.SenderId,
                        c.LastMessage.SenderId.ToString(),
                        c.LastMessage.Text,
                        c.LastMessage.Timestamp)
                    : null,
                c.CreatedAt,
                c.UpdatedAt,
                c.UnreadCount))
            .ToList();

        return Ok(dtos);
    }

    /// <summary>
    /// Marks a conversation as read for the currently authenticated user.
    /// </summary>
    /// <param name="conversationId">The ID of the conversation to mark as read.</param>
    /// <returns>
    /// A <see cref="NoContentResult"/> indicating the operation was successful.
    /// </returns>
    [HttpPost("{conversationId}/mark-as-read")]
    public async Task<IActionResult> MarkConversationAsRead(string conversationId)
    {
        var userId = GetUserId();
        await conversationService.MarkAsReadAsync(conversationId, userId);
        logger.LogInformation("User {UserId} marked conversation {ConversationId} as read.", userId, conversationId);
        return NoContent();
    }
}