<a name='assembly'></a>
# ChatService

## Contents

- [ChatHub](#T-ChatService-Hubs-ChatHub 'ChatService.Hubs.ChatHub')
  - [#ctor()](#M-ChatService-Hubs-ChatHub-#ctor-ChatService-Services-IConversationService,Microsoft-Extensions-Logging-ILogger{ChatService-Hubs-ChatHub}- 'ChatService.Hubs.ChatHub.#ctor(ChatService.Services.IConversationService,Microsoft.Extensions.Logging.ILogger{ChatService.Hubs.ChatHub})')
  - [JoinConversation(conversationId)](#M-ChatService-Hubs-ChatHub-JoinConversation-System-String- 'ChatService.Hubs.ChatHub.JoinConversation(System.String)')
  - [OnConnectedAsync()](#M-ChatService-Hubs-ChatHub-OnConnectedAsync 'ChatService.Hubs.ChatHub.OnConnectedAsync')
  - [SendMessage(conversationId,text)](#M-ChatService-Hubs-ChatHub-SendMessage-System-String,System-String- 'ChatService.Hubs.ChatHub.SendMessage(System.String,System.String)')
- [Conversation](#T-ChatService-Models-Conversation 'ChatService.Models.Conversation')
  - [CreatedAt](#P-ChatService-Models-Conversation-CreatedAt 'ChatService.Models.Conversation.CreatedAt')
  - [Id](#P-ChatService-Models-Conversation-Id 'ChatService.Models.Conversation.Id')
  - [LastMessage](#P-ChatService-Models-Conversation-LastMessage 'ChatService.Models.Conversation.LastMessage')
  - [LastReadTimestamps](#P-ChatService-Models-Conversation-LastReadTimestamps 'ChatService.Models.Conversation.LastReadTimestamps')
  - [ParticipantIds](#P-ChatService-Models-Conversation-ParticipantIds 'ChatService.Models.Conversation.ParticipantIds')
  - [UnreadCount](#P-ChatService-Models-Conversation-UnreadCount 'ChatService.Models.Conversation.UnreadCount')
  - [UpdatedAt](#P-ChatService-Models-Conversation-UpdatedAt 'ChatService.Models.Conversation.UpdatedAt')
- [ConversationDto](#T-ChatService-Models-DTOs-ConversationDto 'ChatService.Models.DTOs.ConversationDto')
  - [#ctor(id,participantIds,lastMessage,createdAt,updatedAt,unreadCount)](#M-ChatService-Models-DTOs-ConversationDto-#ctor-System-String,System-Collections-Generic-List{System-Guid},ChatService-Models-DTOs-LastMessageDto,System-DateTime,System-DateTime,System-Int32- 'ChatService.Models.DTOs.ConversationDto.#ctor(System.String,System.Collections.Generic.List{System.Guid},ChatService.Models.DTOs.LastMessageDto,System.DateTime,System.DateTime,System.Int32)')
  - [CreatedAt](#P-ChatService-Models-DTOs-ConversationDto-CreatedAt 'ChatService.Models.DTOs.ConversationDto.CreatedAt')
  - [Id](#P-ChatService-Models-DTOs-ConversationDto-Id 'ChatService.Models.DTOs.ConversationDto.Id')
  - [LastMessage](#P-ChatService-Models-DTOs-ConversationDto-LastMessage 'ChatService.Models.DTOs.ConversationDto.LastMessage')
  - [ParticipantIds](#P-ChatService-Models-DTOs-ConversationDto-ParticipantIds 'ChatService.Models.DTOs.ConversationDto.ParticipantIds')
  - [UnreadCount](#P-ChatService-Models-DTOs-ConversationDto-UnreadCount 'ChatService.Models.DTOs.ConversationDto.UnreadCount')
  - [UpdatedAt](#P-ChatService-Models-DTOs-ConversationDto-UpdatedAt 'ChatService.Models.DTOs.ConversationDto.UpdatedAt')
- [ConversationService](#T-ChatService-Services-ConversationService 'ChatService.Services.ConversationService')
  - [#ctor()](#M-ChatService-Services-ConversationService-#ctor-MongoDB-Driver-IMongoDatabase- 'ChatService.Services.ConversationService.#ctor(MongoDB.Driver.IMongoDatabase)')
  - [AddMessageAsync(conversationId,senderId,senderTag,text,messageType)](#M-ChatService-Services-ConversationService-AddMessageAsync-System-String,System-Guid,System-String,System-String,System-String- 'ChatService.Services.ConversationService.AddMessageAsync(System.String,System.Guid,System.String,System.String,System.String)')
  - [CreateOrGetConversationAsync(user1Id,user2Id)](#M-ChatService-Services-ConversationService-CreateOrGetConversationAsync-System-Guid,System-Guid- 'ChatService.Services.ConversationService.CreateOrGetConversationAsync(System.Guid,System.Guid)')
  - [EnsureIndexesAsync()](#M-ChatService-Services-ConversationService-EnsureIndexesAsync 'ChatService.Services.ConversationService.EnsureIndexesAsync')
  - [GetByIdAsync(conversationId,requestingUserId)](#M-ChatService-Services-ConversationService-GetByIdAsync-System-String,System-Guid- 'ChatService.Services.ConversationService.GetByIdAsync(System.String,System.Guid)')
  - [GetMessagesAsync(conversationId,requestingUserId,beforeTimestamp,limit)](#M-ChatService-Services-ConversationService-GetMessagesAsync-System-String,System-Guid,System-Nullable{System-DateTime},System-Int32- 'ChatService.Services.ConversationService.GetMessagesAsync(System.String,System.Guid,System.Nullable{System.DateTime},System.Int32)')
  - [GetUserConversationsAsync(userId)](#M-ChatService-Services-ConversationService-GetUserConversationsAsync-System-Guid- 'ChatService.Services.ConversationService.GetUserConversationsAsync(System.Guid)')
  - [MarkAsReadAsync(conversationId,userId)](#M-ChatService-Services-ConversationService-MarkAsReadAsync-System-String,System-Guid- 'ChatService.Services.ConversationService.MarkAsReadAsync(System.String,System.Guid)')
- [ConversationsController](#T-ChatService-Controllers-ConversationsController 'ChatService.Controllers.ConversationsController')
  - [#ctor()](#M-ChatService-Controllers-ConversationsController-#ctor-ChatService-Services-IConversationService,Microsoft-Extensions-Logging-ILogger{ChatService-Controllers-ConversationsController}- 'ChatService.Controllers.ConversationsController.#ctor(ChatService.Services.IConversationService,Microsoft.Extensions.Logging.ILogger{ChatService.Controllers.ConversationsController})')
  - [GetConversations()](#M-ChatService-Controllers-ConversationsController-GetConversations 'ChatService.Controllers.ConversationsController.GetConversations')
  - [GetMessages(conversationId,beforeTimestamp,limit)](#M-ChatService-Controllers-ConversationsController-GetMessages-System-String,System-Nullable{System-DateTime},System-Int32- 'ChatService.Controllers.ConversationsController.GetMessages(System.String,System.Nullable{System.DateTime},System.Int32)')
  - [GetUserId()](#M-ChatService-Controllers-ConversationsController-GetUserId 'ChatService.Controllers.ConversationsController.GetUserId')
  - [GetUserTag()](#M-ChatService-Controllers-ConversationsController-GetUserTag 'ChatService.Controllers.ConversationsController.GetUserTag')
  - [InitiateConversation(request)](#M-ChatService-Controllers-ConversationsController-InitiateConversation-ChatService-Models-DTOs-InitiateConversationRequestDto- 'ChatService.Controllers.ConversationsController.InitiateConversation(ChatService.Models.DTOs.InitiateConversationRequestDto)')
  - [MarkConversationAsRead(conversationId)](#M-ChatService-Controllers-ConversationsController-MarkConversationAsRead-System-String- 'ChatService.Controllers.ConversationsController.MarkConversationAsRead(System.String)')
- [EmbeddedMessage](#T-ChatService-Models-EmbeddedMessage 'ChatService.Models.EmbeddedMessage')
  - [MessageId](#P-ChatService-Models-EmbeddedMessage-MessageId 'ChatService.Models.EmbeddedMessage.MessageId')
  - [SenderId](#P-ChatService-Models-EmbeddedMessage-SenderId 'ChatService.Models.EmbeddedMessage.SenderId')
  - [Text](#P-ChatService-Models-EmbeddedMessage-Text 'ChatService.Models.EmbeddedMessage.Text')
  - [Timestamp](#P-ChatService-Models-EmbeddedMessage-Timestamp 'ChatService.Models.EmbeddedMessage.Timestamp')
- [IConversationService](#T-ChatService-Services-IConversationService 'ChatService.Services.IConversationService')
  - [AddMessageAsync(conversationId,senderId,senderTag,text,messageType)](#M-ChatService-Services-IConversationService-AddMessageAsync-System-String,System-Guid,System-String,System-String,System-String- 'ChatService.Services.IConversationService.AddMessageAsync(System.String,System.Guid,System.String,System.String,System.String)')
  - [CreateOrGetConversationAsync(user1Id,user2Id)](#M-ChatService-Services-IConversationService-CreateOrGetConversationAsync-System-Guid,System-Guid- 'ChatService.Services.IConversationService.CreateOrGetConversationAsync(System.Guid,System.Guid)')
  - [EnsureIndexesAsync()](#M-ChatService-Services-IConversationService-EnsureIndexesAsync 'ChatService.Services.IConversationService.EnsureIndexesAsync')
  - [GetByIdAsync(conversationId,requestingUserId)](#M-ChatService-Services-IConversationService-GetByIdAsync-System-String,System-Guid- 'ChatService.Services.IConversationService.GetByIdAsync(System.String,System.Guid)')
  - [GetMessagesAsync(conversationId,requestingUserId,beforeTimestamp,limit)](#M-ChatService-Services-IConversationService-GetMessagesAsync-System-String,System-Guid,System-Nullable{System-DateTime},System-Int32- 'ChatService.Services.IConversationService.GetMessagesAsync(System.String,System.Guid,System.Nullable{System.DateTime},System.Int32)')
  - [GetUserConversationsAsync(userId)](#M-ChatService-Services-IConversationService-GetUserConversationsAsync-System-Guid- 'ChatService.Services.IConversationService.GetUserConversationsAsync(System.Guid)')
  - [MarkAsReadAsync(conversationId,userId)](#M-ChatService-Services-IConversationService-MarkAsReadAsync-System-String,System-Guid- 'ChatService.Services.IConversationService.MarkAsReadAsync(System.String,System.Guid)')
- [InitiateConversationRequestDto](#T-ChatService-Models-DTOs-InitiateConversationRequestDto 'ChatService.Models.DTOs.InitiateConversationRequestDto')
  - [RecipientId](#P-ChatService-Models-DTOs-InitiateConversationRequestDto-RecipientId 'ChatService.Models.DTOs.InitiateConversationRequestDto.RecipientId')
- [JwtSettings](#T-ChatService-Models-Configuration-JwtSettings 'ChatService.Models.Configuration.JwtSettings')
  - [Audience](#P-ChatService-Models-Configuration-JwtSettings-Audience 'ChatService.Models.Configuration.JwtSettings.Audience')
  - [ExpiryMinutes](#P-ChatService-Models-Configuration-JwtSettings-ExpiryMinutes 'ChatService.Models.Configuration.JwtSettings.ExpiryMinutes')
  - [Issuer](#P-ChatService-Models-Configuration-JwtSettings-Issuer 'ChatService.Models.Configuration.JwtSettings.Issuer')
  - [RefreshTokenExpiryDays](#P-ChatService-Models-Configuration-JwtSettings-RefreshTokenExpiryDays 'ChatService.Models.Configuration.JwtSettings.RefreshTokenExpiryDays')
  - [Secret](#P-ChatService-Models-Configuration-JwtSettings-Secret 'ChatService.Models.Configuration.JwtSettings.Secret')
- [LastMessageDto](#T-ChatService-Models-DTOs-LastMessageDto 'ChatService.Models.DTOs.LastMessageDto')
  - [#ctor(messageId,senderId,senderTag,text,timestamp)](#M-ChatService-Models-DTOs-LastMessageDto-#ctor-System-String,System-Guid,System-String,System-String,System-DateTime- 'ChatService.Models.DTOs.LastMessageDto.#ctor(System.String,System.Guid,System.String,System.String,System.DateTime)')
  - [MessageId](#P-ChatService-Models-DTOs-LastMessageDto-MessageId 'ChatService.Models.DTOs.LastMessageDto.MessageId')
  - [SenderId](#P-ChatService-Models-DTOs-LastMessageDto-SenderId 'ChatService.Models.DTOs.LastMessageDto.SenderId')
  - [SenderTag](#P-ChatService-Models-DTOs-LastMessageDto-SenderTag 'ChatService.Models.DTOs.LastMessageDto.SenderTag')
  - [Text](#P-ChatService-Models-DTOs-LastMessageDto-Text 'ChatService.Models.DTOs.LastMessageDto.Text')
  - [Timestamp](#P-ChatService-Models-DTOs-LastMessageDto-Timestamp 'ChatService.Models.DTOs.LastMessageDto.Timestamp')
- [Message](#T-ChatService-Models-Message 'ChatService.Models.Message')
  - [ConversationId](#P-ChatService-Models-Message-ConversationId 'ChatService.Models.Message.ConversationId')
  - [Id](#P-ChatService-Models-Message-Id 'ChatService.Models.Message.Id')
  - [MessageType](#P-ChatService-Models-Message-MessageType 'ChatService.Models.Message.MessageType')
  - [SenderId](#P-ChatService-Models-Message-SenderId 'ChatService.Models.Message.SenderId')
  - [SenderTag](#P-ChatService-Models-Message-SenderTag 'ChatService.Models.Message.SenderTag')
  - [Text](#P-ChatService-Models-Message-Text 'ChatService.Models.Message.Text')
  - [Timestamp](#P-ChatService-Models-Message-Timestamp 'ChatService.Models.Message.Timestamp')
- [MessageDto](#T-ChatService-Models-DTOs-MessageDto 'ChatService.Models.DTOs.MessageDto')
  - [#ctor(id,conversationId,senderId,senderTag,text,timestamp,messageType)](#M-ChatService-Models-DTOs-MessageDto-#ctor-System-String,System-String,System-Guid,System-String,System-String,System-DateTime,System-String- 'ChatService.Models.DTOs.MessageDto.#ctor(System.String,System.String,System.Guid,System.String,System.String,System.DateTime,System.String)')
  - [ConversationId](#P-ChatService-Models-DTOs-MessageDto-ConversationId 'ChatService.Models.DTOs.MessageDto.ConversationId')
  - [Id](#P-ChatService-Models-DTOs-MessageDto-Id 'ChatService.Models.DTOs.MessageDto.Id')
  - [MessageType](#P-ChatService-Models-DTOs-MessageDto-MessageType 'ChatService.Models.DTOs.MessageDto.MessageType')
  - [SenderId](#P-ChatService-Models-DTOs-MessageDto-SenderId 'ChatService.Models.DTOs.MessageDto.SenderId')
  - [SenderTag](#P-ChatService-Models-DTOs-MessageDto-SenderTag 'ChatService.Models.DTOs.MessageDto.SenderTag')
  - [Text](#P-ChatService-Models-DTOs-MessageDto-Text 'ChatService.Models.DTOs.MessageDto.Text')
  - [Timestamp](#P-ChatService-Models-DTOs-MessageDto-Timestamp 'ChatService.Models.DTOs.MessageDto.Timestamp')
- [MongoDbSettings](#T-ChatService-Models-Configuration-MongoDbSettings 'ChatService.Models.Configuration.MongoDbSettings')
  - [MongoConnection](#P-ChatService-Models-Configuration-MongoDbSettings-MongoConnection 'ChatService.Models.Configuration.MongoDbSettings.MongoConnection')
  - [MongoDatabaseName](#P-ChatService-Models-Configuration-MongoDbSettings-MongoDatabaseName 'ChatService.Models.Configuration.MongoDbSettings.MongoDatabaseName')

<a name='T-ChatService-Hubs-ChatHub'></a>
## ChatHub `type`

##### Namespace

ChatService.Hubs

##### Summary

SignalR hub for managing real-time chat functionality.

<a name='M-ChatService-Hubs-ChatHub-#ctor-ChatService-Services-IConversationService,Microsoft-Extensions-Logging-ILogger{ChatService-Hubs-ChatHub}-'></a>
### #ctor() `constructor`

##### Summary

SignalR hub for managing real-time chat functionality.

##### Parameters

This constructor has no parameters.

<a name='M-ChatService-Hubs-ChatHub-JoinConversation-System-String-'></a>
### JoinConversation(conversationId) `method`

##### Summary

Adds the current connection to a SignalR group representing a conversation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation to join. |

<a name='M-ChatService-Hubs-ChatHub-OnConnectedAsync'></a>
### OnConnectedAsync() `method`

##### Summary

Handles the event when a client connects to the hub.
Adds the connection to the user's SignalR group if authenticated.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Hubs-ChatHub-SendMessage-System-String,System-String-'></a>
### SendMessage(conversationId,text) `method`

##### Summary

Sends a message to all participants in a conversation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text content of the message. |

<a name='T-ChatService-Models-Conversation'></a>
## Conversation `type`

##### Namespace

ChatService.Models

##### Summary

Represents a conversation between participants in the chat service.

<a name='P-ChatService-Models-Conversation-CreatedAt'></a>
### CreatedAt `property`

##### Summary

Gets or sets the timestamp indicating when the conversation was created.
Defaults to the current UTC time.

<a name='P-ChatService-Models-Conversation-Id'></a>
### Id `property`

##### Summary

Gets or sets the unique identifier for the conversation.

<a name='P-ChatService-Models-Conversation-LastMessage'></a>
### LastMessage `property`

##### Summary

Gets or sets the last message in the conversation.

<a name='P-ChatService-Models-Conversation-LastReadTimestamps'></a>
### LastReadTimestamps `property`

##### Summary

Gets or sets the dictionary of last read timestamps for each participant.
The key is the participant's ID as a string, and the value is the timestamp.

<a name='P-ChatService-Models-Conversation-ParticipantIds'></a>
### ParticipantIds `property`

##### Summary

Gets or sets the list of participant IDs in the conversation.

<a name='P-ChatService-Models-Conversation-UnreadCount'></a>
### UnreadCount `property`

##### Summary

Gets or sets the count of unread messages in the conversation.
This property is ignored by MongoDB.

<a name='P-ChatService-Models-Conversation-UpdatedAt'></a>
### UpdatedAt `property`

##### Summary

Gets or sets the timestamp indicating when the conversation was last updated.
Defaults to the current UTC time.

<a name='T-ChatService-Models-DTOs-ConversationDto'></a>
## ConversationDto `type`

##### Namespace

ChatService.Models.DTOs

##### Summary

Represents a conversation data transfer object (DTO).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [T:ChatService.Models.DTOs.ConversationDto](#T-T-ChatService-Models-DTOs-ConversationDto 'T:ChatService.Models.DTOs.ConversationDto') | The unique identifier of the conversation. |

<a name='M-ChatService-Models-DTOs-ConversationDto-#ctor-System-String,System-Collections-Generic-List{System-Guid},ChatService-Models-DTOs-LastMessageDto,System-DateTime,System-DateTime,System-Int32-'></a>
### #ctor(id,participantIds,lastMessage,createdAt,updatedAt,unreadCount) `constructor`

##### Summary

Represents a conversation data transfer object (DTO).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The unique identifier of the conversation. |
| participantIds | [System.Collections.Generic.List{System.Guid}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{System.Guid}') | The list of participant IDs in the conversation. |
| lastMessage | [ChatService.Models.DTOs.LastMessageDto](#T-ChatService-Models-DTOs-LastMessageDto 'ChatService.Models.DTOs.LastMessageDto') | The last message in the conversation, if available. |
| createdAt | [System.DateTime](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.DateTime 'System.DateTime') | The timestamp when the conversation was created. |
| updatedAt | [System.DateTime](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.DateTime 'System.DateTime') | The timestamp when the conversation was last updated. |
| unreadCount | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number of unread messages in the conversation. |

<a name='P-ChatService-Models-DTOs-ConversationDto-CreatedAt'></a>
### CreatedAt `property`

##### Summary

Gets or sets the timestamp when the conversation was created.

<a name='P-ChatService-Models-DTOs-ConversationDto-Id'></a>
### Id `property`

##### Summary

Gets or sets the unique identifier of the conversation.

<a name='P-ChatService-Models-DTOs-ConversationDto-LastMessage'></a>
### LastMessage `property`

##### Summary

Gets or sets the last message in the conversation, if available.

<a name='P-ChatService-Models-DTOs-ConversationDto-ParticipantIds'></a>
### ParticipantIds `property`

##### Summary

Gets or sets the list of participant IDs in the conversation.

<a name='P-ChatService-Models-DTOs-ConversationDto-UnreadCount'></a>
### UnreadCount `property`

##### Summary

Gets or sets the number of unread messages in the conversation.

<a name='P-ChatService-Models-DTOs-ConversationDto-UpdatedAt'></a>
### UpdatedAt `property`

##### Summary

Gets or sets the timestamp when the conversation was last updated.

<a name='T-ChatService-Services-ConversationService'></a>
## ConversationService `type`

##### Namespace

ChatService.Services

##### Summary

Service for managing conversations and messages in the chat application.

<a name='M-ChatService-Services-ConversationService-#ctor-MongoDB-Driver-IMongoDatabase-'></a>
### #ctor() `constructor`

##### Summary

Service for managing conversations and messages in the chat application.

##### Parameters

This constructor has no parameters.

<a name='M-ChatService-Services-ConversationService-AddMessageAsync-System-String,System-Guid,System-String,System-String,System-String-'></a>
### AddMessageAsync(conversationId,senderId,senderTag,text,messageType) `method`

##### Summary

Adds a new message to a conversation and updates the conversation's last message and timestamp.

##### Returns

The added message object.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| senderId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the sender. |
| senderTag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The tag of the sender. |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text of the message. |
| messageType | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The type of the message (default is "text"). |

<a name='M-ChatService-Services-ConversationService-CreateOrGetConversationAsync-System-Guid,System-Guid-'></a>
### CreateOrGetConversationAsync(user1Id,user2Id) `method`

##### Summary

Creates a new conversation between two users or retrieves an existing one.

##### Returns

The conversation object.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| user1Id | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the first user. |
| user2Id | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the second user. |

<a name='M-ChatService-Services-ConversationService-EnsureIndexesAsync'></a>
### EnsureIndexesAsync() `method`

##### Summary

Ensures necessary indexes are created in the database for conversations and messages.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Services-ConversationService-GetByIdAsync-System-String,System-Guid-'></a>
### GetByIdAsync(conversationId,requestingUserId) `method`

##### Summary

Retrieves a conversation by its ID.

##### Returns

The conversation object.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| requestingUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user requesting the conversation. |

<a name='M-ChatService-Services-ConversationService-GetMessagesAsync-System-String,System-Guid,System-Nullable{System-DateTime},System-Int32-'></a>
### GetMessagesAsync(conversationId,requestingUserId,beforeTimestamp,limit) `method`

##### Summary

Retrieves messages from a conversation, optionally filtered by timestamp.

##### Returns

A list of messages.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| requestingUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user requesting the messages. |
| beforeTimestamp | [System.Nullable{System.DateTime}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.DateTime}') | Optional timestamp to retrieve messages before a specific time. |
| limit | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The maximum number of messages to retrieve. |

<a name='M-ChatService-Services-ConversationService-GetUserConversationsAsync-System-Guid-'></a>
### GetUserConversationsAsync(userId) `method`

##### Summary

Retrieves all conversations for a specific user, including unread message counts.

##### Returns

A list of conversations.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user. |

<a name='M-ChatService-Services-ConversationService-MarkAsReadAsync-System-String,System-Guid-'></a>
### MarkAsReadAsync(conversationId,userId) `method`

##### Summary

Marks a conversation as read for a specific user.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user. |

<a name='T-ChatService-Controllers-ConversationsController'></a>
## ConversationsController `type`

##### Namespace

ChatService.Controllers

##### Summary

Controller for managing conversations and related operations.
Requires authorization for all endpoints.

<a name='M-ChatService-Controllers-ConversationsController-#ctor-ChatService-Services-IConversationService,Microsoft-Extensions-Logging-ILogger{ChatService-Controllers-ConversationsController}-'></a>
### #ctor() `constructor`

##### Summary

Controller for managing conversations and related operations.
Requires authorization for all endpoints.

##### Parameters

This constructor has no parameters.

<a name='M-ChatService-Controllers-ConversationsController-GetConversations'></a>
### GetConversations() `method`

##### Summary

Retrieves all conversations for the currently authenticated user.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the list of conversations.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Controllers-ConversationsController-GetMessages-System-String,System-Nullable{System-DateTime},System-Int32-'></a>
### GetMessages(conversationId,beforeTimestamp,limit) `method`

##### Summary

Retrieves messages for a specific conversation.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the list of messages in the conversation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| beforeTimestamp | [System.Nullable{System.DateTime}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.DateTime}') | Optional timestamp to fetch messages before. |
| limit | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The maximum number of messages to retrieve. Defaults to 20. |

<a name='M-ChatService-Controllers-ConversationsController-GetUserId'></a>
### GetUserId() `method`

##### Summary

Retrieves the user ID from the token claims.

##### Returns

The user ID as a [Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid').

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | Thrown if the user ID is not found in the token claims. |

<a name='M-ChatService-Controllers-ConversationsController-GetUserTag'></a>
### GetUserTag() `method`

##### Summary

Retrieves the user tag from the token claims.

##### Returns

The user tag as a [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String'). Defaults to "UnknownUser" if not found.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Controllers-ConversationsController-InitiateConversation-ChatService-Models-DTOs-InitiateConversationRequestDto-'></a>
### InitiateConversation(request) `method`

##### Summary

Initiates a new conversation or retrieves an existing one.

##### Returns

An [IActionResult](#T-Microsoft-AspNetCore-Mvc-IActionResult 'Microsoft.AspNetCore.Mvc.IActionResult') containing the conversation details or an error response.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| request | [ChatService.Models.DTOs.InitiateConversationRequestDto](#T-ChatService-Models-DTOs-InitiateConversationRequestDto 'ChatService.Models.DTOs.InitiateConversationRequestDto') | The request containing the recipient ID. |

<a name='M-ChatService-Controllers-ConversationsController-MarkConversationAsRead-System-String-'></a>
### MarkConversationAsRead(conversationId) `method`

##### Summary

Marks a conversation as read for the currently authenticated user.

##### Returns

A [NoContentResult](#T-Microsoft-AspNetCore-Mvc-NoContentResult 'Microsoft.AspNetCore.Mvc.NoContentResult') indicating the operation was successful.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation to mark as read. |

<a name='T-ChatService-Models-EmbeddedMessage'></a>
## EmbeddedMessage `type`

##### Namespace

ChatService.Models

##### Summary

Represents an embedded message within a conversation.

<a name='P-ChatService-Models-EmbeddedMessage-MessageId'></a>
### MessageId `property`

##### Summary

Gets or sets the unique identifier for the message.

<a name='P-ChatService-Models-EmbeddedMessage-SenderId'></a>
### SenderId `property`

##### Summary

Gets or sets the unique identifier of the sender of the message.

<a name='P-ChatService-Models-EmbeddedMessage-Text'></a>
### Text `property`

##### Summary

Gets or sets the text content of the message.

<a name='P-ChatService-Models-EmbeddedMessage-Timestamp'></a>
### Timestamp `property`

##### Summary

Gets or sets the timestamp indicating when the message was created.

<a name='T-ChatService-Services-IConversationService'></a>
## IConversationService `type`

##### Namespace

ChatService.Services

##### Summary

Interface for managing conversations and messages in the chat service.

<a name='M-ChatService-Services-IConversationService-AddMessageAsync-System-String,System-Guid,System-String,System-String,System-String-'></a>
### AddMessageAsync(conversationId,senderId,senderTag,text,messageType) `method`

##### Summary

Adds a new message to a conversation.

##### Returns

A task that represents the asynchronous operation. The task result contains the added message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| senderId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the sender. |
| senderTag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The tag of the sender. |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text of the message. |
| messageType | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The type of the message (default is "text"). |

<a name='M-ChatService-Services-IConversationService-CreateOrGetConversationAsync-System-Guid,System-Guid-'></a>
### CreateOrGetConversationAsync(user1Id,user2Id) `method`

##### Summary

Creates a new conversation between two users or retrieves an existing one.

##### Returns

A task that represents the asynchronous operation. The task result contains the conversation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| user1Id | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the first user. |
| user2Id | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the second user. |

<a name='M-ChatService-Services-IConversationService-EnsureIndexesAsync'></a>
### EnsureIndexesAsync() `method`

##### Summary

Ensures that necessary indexes are created in the database.

##### Returns

A task that represents the asynchronous operation.

##### Parameters

This method has no parameters.

<a name='M-ChatService-Services-IConversationService-GetByIdAsync-System-String,System-Guid-'></a>
### GetByIdAsync(conversationId,requestingUserId) `method`

##### Summary

Retrieves a conversation by its ID.

##### Returns

A task that represents the asynchronous operation. The task result contains the conversation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| requestingUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user requesting the conversation. |

<a name='M-ChatService-Services-IConversationService-GetMessagesAsync-System-String,System-Guid,System-Nullable{System-DateTime},System-Int32-'></a>
### GetMessagesAsync(conversationId,requestingUserId,beforeTimestamp,limit) `method`

##### Summary

Retrieves messages from a conversation.

##### Returns

A task that represents the asynchronous operation. The task result contains a list of messages.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| requestingUserId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user requesting the messages. |
| beforeTimestamp | [System.Nullable{System.DateTime}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.DateTime}') | Optional timestamp to retrieve messages before a specific time. |
| limit | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The maximum number of messages to retrieve. |

<a name='M-ChatService-Services-IConversationService-GetUserConversationsAsync-System-Guid-'></a>
### GetUserConversationsAsync(userId) `method`

##### Summary

Retrieves all conversations for a specific user.

##### Returns

A task that represents the asynchronous operation. The task result contains a list of conversations.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user. |

<a name='M-ChatService-Services-IConversationService-MarkAsReadAsync-System-String,System-Guid-'></a>
### MarkAsReadAsync(conversationId,userId) `method`

##### Summary

Marks a conversation as read for a specific user.

##### Returns

A task that represents the asynchronous operation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ID of the conversation. |
| userId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The ID of the user. |

<a name='T-ChatService-Models-DTOs-InitiateConversationRequestDto'></a>
## InitiateConversationRequestDto `type`

##### Namespace

ChatService.Models.DTOs

##### Summary

Represents a request to initiate a conversation.

<a name='P-ChatService-Models-DTOs-InitiateConversationRequestDto-RecipientId'></a>
### RecipientId `property`

##### Summary

Gets or sets the unique identifier of the recipient with whom the conversation is to be initiated.

<a name='T-ChatService-Models-Configuration-JwtSettings'></a>
## JwtSettings `type`

##### Namespace

ChatService.Models.Configuration

##### Summary

Represents the settings required for JWT (JSON Web Token) authentication.

<a name='P-ChatService-Models-Configuration-JwtSettings-Audience'></a>
### Audience `property`

##### Summary

Gets or sets the audience for the JWTs.

<a name='P-ChatService-Models-Configuration-JwtSettings-ExpiryMinutes'></a>
### ExpiryMinutes `property`

##### Summary

Gets or sets the expiration time for access tokens, in minutes.

<a name='P-ChatService-Models-Configuration-JwtSettings-Issuer'></a>
### Issuer `property`

##### Summary

Gets or sets the issuer of the JWTs.

<a name='P-ChatService-Models-Configuration-JwtSettings-RefreshTokenExpiryDays'></a>
### RefreshTokenExpiryDays `property`

##### Summary

Gets or sets the expiration time for refresh tokens, in days.

<a name='P-ChatService-Models-Configuration-JwtSettings-Secret'></a>
### Secret `property`

##### Summary

Gets or sets the secret key used for signing JWTs.

<a name='T-ChatService-Models-DTOs-LastMessageDto'></a>
## LastMessageDto `type`

##### Namespace

ChatService.Models.DTOs

##### Summary

Represents the last message in a conversation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| messageId | [T:ChatService.Models.DTOs.LastMessageDto](#T-T-ChatService-Models-DTOs-LastMessageDto 'T:ChatService.Models.DTOs.LastMessageDto') | The unique identifier of the message. |

<a name='M-ChatService-Models-DTOs-LastMessageDto-#ctor-System-String,System-Guid,System-String,System-String,System-DateTime-'></a>
### #ctor(messageId,senderId,senderTag,text,timestamp) `constructor`

##### Summary

Represents the last message in a conversation.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| messageId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The unique identifier of the message. |
| senderId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the sender. |
| senderTag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The tag or username of the sender. |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The content of the message. |
| timestamp | [System.DateTime](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.DateTime 'System.DateTime') | The timestamp when the message was sent. |

<a name='P-ChatService-Models-DTOs-LastMessageDto-MessageId'></a>
### MessageId `property`

##### Summary

Gets or sets the unique identifier of the message.

<a name='P-ChatService-Models-DTOs-LastMessageDto-SenderId'></a>
### SenderId `property`

##### Summary

Gets or sets the unique identifier of the sender.

<a name='P-ChatService-Models-DTOs-LastMessageDto-SenderTag'></a>
### SenderTag `property`

##### Summary

Gets or sets the tag or username of the sender.

<a name='P-ChatService-Models-DTOs-LastMessageDto-Text'></a>
### Text `property`

##### Summary

Gets or sets the content of the message.

<a name='P-ChatService-Models-DTOs-LastMessageDto-Timestamp'></a>
### Timestamp `property`

##### Summary

Gets or sets the timestamp when the message was sent.

<a name='T-ChatService-Models-Message'></a>
## Message `type`

##### Namespace

ChatService.Models

##### Summary

Represents a message in a chat conversation.

<a name='P-ChatService-Models-Message-ConversationId'></a>
### ConversationId `property`

##### Summary

Gets or sets the identifier of the conversation to which the message belongs.

<a name='P-ChatService-Models-Message-Id'></a>
### Id `property`

##### Summary

Gets or sets the unique identifier for the message.

<a name='P-ChatService-Models-Message-MessageType'></a>
### MessageType `property`

##### Summary

Gets or sets the type of the message (e.g., "text", "image").
Defaults to "text".

<a name='P-ChatService-Models-Message-SenderId'></a>
### SenderId `property`

##### Summary

Gets or sets the unique identifier of the sender of the message.

<a name='P-ChatService-Models-Message-SenderTag'></a>
### SenderTag `property`

##### Summary

Gets or sets the tag of the sender (e.g., username or display name).

<a name='P-ChatService-Models-Message-Text'></a>
### Text `property`

##### Summary

Gets or sets the text content of the message.

<a name='P-ChatService-Models-Message-Timestamp'></a>
### Timestamp `property`

##### Summary

Gets or sets the timestamp indicating when the message was created.
Defaults to the current UTC time.

<a name='T-ChatService-Models-DTOs-MessageDto'></a>
## MessageDto `type`

##### Namespace

ChatService.Models.DTOs

##### Summary

Represents a message data transfer object (DTO).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [T:ChatService.Models.DTOs.MessageDto](#T-T-ChatService-Models-DTOs-MessageDto 'T:ChatService.Models.DTOs.MessageDto') | The unique identifier of the message. |

<a name='M-ChatService-Models-DTOs-MessageDto-#ctor-System-String,System-String,System-Guid,System-String,System-String,System-DateTime,System-String-'></a>
### #ctor(id,conversationId,senderId,senderTag,text,timestamp,messageType) `constructor`

##### Summary

Represents a message data transfer object (DTO).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The unique identifier of the message. |
| conversationId | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The unique identifier of the conversation the message belongs to. |
| senderId | [System.Guid](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Guid 'System.Guid') | The unique identifier of the sender of the message. |
| senderTag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The tag or username of the sender. |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The content of the message. |
| timestamp | [System.DateTime](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.DateTime 'System.DateTime') | The timestamp when the message was sent. |
| messageType | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The type of the message (e.g., text, image, etc.). Defaults to "text" if not specified. |

<a name='P-ChatService-Models-DTOs-MessageDto-ConversationId'></a>
### ConversationId `property`

##### Summary

Gets or sets the unique identifier of the conversation the message belongs to.

<a name='P-ChatService-Models-DTOs-MessageDto-Id'></a>
### Id `property`

##### Summary

Gets or sets the unique identifier of the message.

<a name='P-ChatService-Models-DTOs-MessageDto-MessageType'></a>
### MessageType `property`

##### Summary

Gets or sets the type of the message (e.g., text, image, etc.). Defaults to "text" if not specified.

<a name='P-ChatService-Models-DTOs-MessageDto-SenderId'></a>
### SenderId `property`

##### Summary

Gets or sets the unique identifier of the sender of the message.

<a name='P-ChatService-Models-DTOs-MessageDto-SenderTag'></a>
### SenderTag `property`

##### Summary

Gets or sets the tag or username of the sender.

<a name='P-ChatService-Models-DTOs-MessageDto-Text'></a>
### Text `property`

##### Summary

Gets or sets the content of the message.

<a name='P-ChatService-Models-DTOs-MessageDto-Timestamp'></a>
### Timestamp `property`

##### Summary

Gets or sets the timestamp when the message was sent.

<a name='T-ChatService-Models-Configuration-MongoDbSettings'></a>
## MongoDbSettings `type`

##### Namespace

ChatService.Models.Configuration

##### Summary

Represents the settings required for configuring MongoDB.

<a name='P-ChatService-Models-Configuration-MongoDbSettings-MongoConnection'></a>
### MongoConnection `property`

##### Summary

Gets or sets the connection string for MongoDB.

<a name='P-ChatService-Models-Configuration-MongoDbSettings-MongoDatabaseName'></a>
### MongoDatabaseName `property`

##### Summary

Gets or sets the name of the MongoDB database.
