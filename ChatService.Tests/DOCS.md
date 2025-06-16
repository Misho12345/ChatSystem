<a name='assembly'></a>
# ChatService.Tests

## Contents

- [ChatHubTest](#T-ChatService-Tests-Hubs-ChatHubTest 'ChatService.Tests.Hubs.ChatHubTest')
  - [#ctor()](#M-ChatService-Tests-Hubs-ChatHubTest-#ctor 'ChatService.Tests.Hubs.ChatHubTest.#ctor')
  - [JoinConversation_AddsConnectionToGroup()](#M-ChatService-Tests-Hubs-ChatHubTest-JoinConversation_AddsConnectionToGroup 'ChatService.Tests.Hubs.ChatHubTest.JoinConversation_AddsConnectionToGroup')
  - [OnConnectedAsync_AddsUserToGroup()](#M-ChatService-Tests-Hubs-ChatHubTest-OnConnectedAsync_AddsUserToGroup 'ChatService.Tests.Hubs.ChatHubTest.OnConnectedAsync_AddsUserToGroup')
  - [SendMessage_WhenConversationAndMessageAreValid_SendsToAllParticipants()](#M-ChatService-Tests-Hubs-ChatHubTest-SendMessage_WhenConversationAndMessageAreValid_SendsToAllParticipants 'ChatService.Tests.Hubs.ChatHubTest.SendMessage_WhenConversationAndMessageAreValid_SendsToAllParticipants')
  - [SendMessage_WhenConversationNotFound_LogsErrorAndDoesNotSend()](#M-ChatService-Tests-Hubs-ChatHubTest-SendMessage_WhenConversationNotFound_LogsErrorAndDoesNotSend 'ChatService.Tests.Hubs.ChatHubTest.SendMessage_WhenConversationNotFound_LogsErrorAndDoesNotSend')
- [ConversationServiceTest](#T-ChatService-Tests-Services-ConversationServiceTest 'ChatService.Tests.Services.ConversationServiceTest')
  - [#ctor()](#M-ChatService-Tests-Services-ConversationServiceTest-#ctor 'ChatService.Tests.Services.ConversationServiceTest.#ctor')
  - [AddMessageAsync_InsertsMessageAndUpdatesConversation()](#M-ChatService-Tests-Services-ConversationServiceTest-AddMessageAsync_InsertsMessageAndUpdatesConversation 'ChatService.Tests.Services.ConversationServiceTest.AddMessageAsync_InsertsMessageAndUpdatesConversation')
  - [CreateOrGetConversationAsync_CreatesAndReturnsSame()](#M-ChatService-Tests-Services-ConversationServiceTest-CreateOrGetConversationAsync_CreatesAndReturnsSame 'ChatService.Tests.Services.ConversationServiceTest.CreateOrGetConversationAsync_CreatesAndReturnsSame')
  - [Dispose()](#M-ChatService-Tests-Services-ConversationServiceTest-Dispose 'ChatService.Tests.Services.ConversationServiceTest.Dispose')
  - [EnsureIndexesAsync_DoesNotThrow()](#M-ChatService-Tests-Services-ConversationServiceTest-EnsureIndexesAsync_DoesNotThrow 'ChatService.Tests.Services.ConversationServiceTest.EnsureIndexesAsync_DoesNotThrow')
  - [GetByIdAsync_ReturnsAndThrowsOnUnauthorized()](#M-ChatService-Tests-Services-ConversationServiceTest-GetByIdAsync_ReturnsAndThrowsOnUnauthorized 'ChatService.Tests.Services.ConversationServiceTest.GetByIdAsync_ReturnsAndThrowsOnUnauthorized')
  - [GetMessagesAsync_ReturnsDescendingAndHonorsLimitAndTimestamp()](#M-ChatService-Tests-Services-ConversationServiceTest-GetMessagesAsync_ReturnsDescendingAndHonorsLimitAndTimestamp 'ChatService.Tests.Services.ConversationServiceTest.GetMessagesAsync_ReturnsDescendingAndHonorsLimitAndTimestamp')
  - [GetMessagesAsync_ThrowsWhenUnauthorized()](#M-ChatService-Tests-Services-ConversationServiceTest-GetMessagesAsync_ThrowsWhenUnauthorized 'ChatService.Tests.Services.ConversationServiceTest.GetMessagesAsync_ThrowsWhenUnauthorized')
  - [GetUserConversationsAsync_FiltersAndSorts()](#M-ChatService-Tests-Services-ConversationServiceTest-GetUserConversationsAsync_FiltersAndSorts 'ChatService.Tests.Services.ConversationServiceTest.GetUserConversationsAsync_FiltersAndSorts')
- [ConversationsControllerTest](#T-ChatService-Tests-Controllers-ConversationsControllerTest 'ChatService.Tests.Controllers.ConversationsControllerTest')
  - [#ctor()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-#ctor 'ChatService.Tests.Controllers.ConversationsControllerTest.#ctor')
  - [GetConversations_WhenUserHasConversations_ReturnsOkWithListOfConversationDtos()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-GetConversations_WhenUserHasConversations_ReturnsOkWithListOfConversationDtos 'ChatService.Tests.Controllers.ConversationsControllerTest.GetConversations_WhenUserHasConversations_ReturnsOkWithListOfConversationDtos')
  - [GetMessages_WhenConversationExists_ReturnsOkWithMessages()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-GetMessages_WhenConversationExists_ReturnsOkWithMessages 'ChatService.Tests.Controllers.ConversationsControllerTest.GetMessages_WhenConversationExists_ReturnsOkWithMessages')
  - [GetMessages_WhenServiceThrowsUnauthorized_ShouldPropagateException()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-GetMessages_WhenServiceThrowsUnauthorized_ShouldPropagateException 'ChatService.Tests.Controllers.ConversationsControllerTest.GetMessages_WhenServiceThrowsUnauthorized_ShouldPropagateException')
  - [InitiateConversation_WhenServiceReturnsNull_ReturnsInternalServerError()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-InitiateConversation_WhenServiceReturnsNull_ReturnsInternalServerError 'ChatService.Tests.Controllers.ConversationsControllerTest.InitiateConversation_WhenServiceReturnsNull_ReturnsInternalServerError')
  - [InitiateConversation_WithSelfAsRecipient_ReturnsBadRequest()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-InitiateConversation_WithSelfAsRecipient_ReturnsBadRequest 'ChatService.Tests.Controllers.ConversationsControllerTest.InitiateConversation_WithSelfAsRecipient_ReturnsBadRequest')
  - [InitiateConversation_WithValidRecipient_ReturnsOkWithConversationDto()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-InitiateConversation_WithValidRecipient_ReturnsOkWithConversationDto 'ChatService.Tests.Controllers.ConversationsControllerTest.InitiateConversation_WithValidRecipient_ReturnsOkWithConversationDto')
  - [MarkConversationAsRead_WithValidRequest_CallsServiceAndReturnsNoContent()](#M-ChatService-Tests-Controllers-ConversationsControllerTest-MarkConversationAsRead_WithValidRequest_CallsServiceAndReturnsNoContent 'ChatService.Tests.Controllers.ConversationsControllerTest.MarkConversationAsRead_WithValidRequest_CallsServiceAndReturnsNoContent')

<a name='T-ChatService-Tests-Hubs-ChatHubTest'></a>
## ChatHubTest `type`

##### Namespace

ChatService.Tests.Hubs

##### Summary

Unit tests for the ChatHub class, which handles SignalR communication for chat functionality.

<a name='M-ChatService-Tests-Hubs-ChatHubTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes the test class by setting up mocks and creating an instance of ChatHub.

##### Parameters

This constructor has no parameters.

<a name='M-ChatService-Tests-Hubs-ChatHubTest-JoinConversation_AddsConnectionToGroup'></a>
### JoinConversation_AddsConnectionToGroup() `method`

##### Summary

Tests that JoinConversation adds the connection to the specified group.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Hubs-ChatHubTest-OnConnectedAsync_AddsUserToGroup'></a>
### OnConnectedAsync_AddsUserToGroup() `method`

##### Summary

Tests that OnConnectedAsync adds the user to their own group based on their user ID.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Hubs-ChatHubTest-SendMessage_WhenConversationAndMessageAreValid_SendsToAllParticipants'></a>
### SendMessage_WhenConversationAndMessageAreValid_SendsToAllParticipants() `method`

##### Summary

Tests that SendMessage sends a message to all participants in a valid conversation.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Hubs-ChatHubTest-SendMessage_WhenConversationNotFound_LogsErrorAndDoesNotSend'></a>
### SendMessage_WhenConversationNotFound_LogsErrorAndDoesNotSend() `method`

##### Summary

Tests that SendMessage logs an error and does not send a message when the conversation is not found.

##### Parameters

This method has no parameters.

<a name='T-ChatService-Tests-Services-ConversationServiceTest'></a>
## ConversationServiceTest `type`

##### Namespace

ChatService.Tests.Services

##### Summary

Unit tests for the ConversationService class, which handles database operations for conversations and messages.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes the test class by setting up a MongoDB instance and creating a ConversationService instance.

##### Parameters

This constructor has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-AddMessageAsync_InsertsMessageAndUpdatesConversation'></a>
### AddMessageAsync_InsertsMessageAndUpdatesConversation() `method`

##### Summary

Tests that AddMessageAsync inserts a message and updates the conversation's last message.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-CreateOrGetConversationAsync_CreatesAndReturnsSame'></a>
### CreateOrGetConversationAsync_CreatesAndReturnsSame() `method`

##### Summary

Tests that CreateOrGetConversationAsync creates a new conversation or retrieves an existing one.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-Dispose'></a>
### Dispose() `method`

##### Summary

Disposes of the MongoDB runner instance if it was created.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-EnsureIndexesAsync_DoesNotThrow'></a>
### EnsureIndexesAsync_DoesNotThrow() `method`

##### Summary

Tests that EnsureIndexesAsync does not throw any exceptions.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-GetByIdAsync_ReturnsAndThrowsOnUnauthorized'></a>
### GetByIdAsync_ReturnsAndThrowsOnUnauthorized() `method`

##### Summary

Tests that GetByIdAsync returns the conversation for authorized users and throws for unauthorized users.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-GetMessagesAsync_ReturnsDescendingAndHonorsLimitAndTimestamp'></a>
### GetMessagesAsync_ReturnsDescendingAndHonorsLimitAndTimestamp() `method`

##### Summary

Tests that GetMessagesAsync returns messages in descending order and respects limit and timestamp filters.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-GetMessagesAsync_ThrowsWhenUnauthorized'></a>
### GetMessagesAsync_ThrowsWhenUnauthorized() `method`

##### Summary

Tests that GetMessagesAsync throws an UnauthorizedAccessException when the user is not a participant.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Services-ConversationServiceTest-GetUserConversationsAsync_FiltersAndSorts'></a>
### GetUserConversationsAsync_FiltersAndSorts() `method`

##### Summary

Tests that GetUserConversationsAsync filters conversations by user and sorts them by last message timestamp.

##### Parameters

This method has no parameters.

<a name='T-ChatService-Tests-Controllers-ConversationsControllerTest'></a>
## ConversationsControllerTest `type`

##### Namespace

ChatService.Tests.Controllers

##### Summary

Unit tests for the ConversationsController class.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes the ConversationsControllerTest class and sets up mock dependencies.

##### Parameters

This constructor has no parameters.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-GetConversations_WhenUserHasConversations_ReturnsOkWithListOfConversationDtos'></a>
### GetConversations_WhenUserHasConversations_ReturnsOkWithListOfConversationDtos() `method`

##### Summary

Tests that GetConversations returns an Ok result with a list of ConversationDtos when the user has conversations.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-GetMessages_WhenConversationExists_ReturnsOkWithMessages'></a>
### GetMessages_WhenConversationExists_ReturnsOkWithMessages() `method`

##### Summary

Tests that GetMessages returns an Ok result with messages when the conversation exists.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-GetMessages_WhenServiceThrowsUnauthorized_ShouldPropagateException'></a>
### GetMessages_WhenServiceThrowsUnauthorized_ShouldPropagateException() `method`

##### Summary

Tests that GetMessages propagates an UnauthorizedAccessException when the service throws it.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-InitiateConversation_WhenServiceReturnsNull_ReturnsInternalServerError'></a>
### InitiateConversation_WhenServiceReturnsNull_ReturnsInternalServerError() `method`

##### Summary

Tests that InitiateConversation returns an InternalServerError result when the service returns null.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-InitiateConversation_WithSelfAsRecipient_ReturnsBadRequest'></a>
### InitiateConversation_WithSelfAsRecipient_ReturnsBadRequest() `method`

##### Summary

Tests that InitiateConversation returns a BadRequest result when the recipient is the same as the sender.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-InitiateConversation_WithValidRecipient_ReturnsOkWithConversationDto'></a>
### InitiateConversation_WithValidRecipient_ReturnsOkWithConversationDto() `method`

##### Summary

Tests that InitiateConversation returns an Ok result with a ConversationDto when a valid recipient is provided.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Tests-Controllers-ConversationsControllerTest-MarkConversationAsRead_WithValidRequest_CallsServiceAndReturnsNoContent'></a>
### MarkConversationAsRead_WithValidRequest_CallsServiceAndReturnsNoContent() `method`

##### Summary

Tests that MarkConversationAsRead calls the service and returns a NoContent result when the request is valid.

##### Parameters

This method has no parameters.
