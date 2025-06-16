/**
 * Module for managing SignalR connection to the ChatHub and handling real-time chat functionality.
 */
const ChatClient = (function () {
    const CHAT_HUB_URL = 'http://localhost:8002/chathub';
    let connection = null;

    /**
     * Starts the SignalR connection to the ChatHub.
     * Reuses the existing connection if already established.
     * @param {string} accessToken - The access token for authentication.
     * @returns {Promise<signalR.HubConnection>} - Resolves with the SignalR connection instance.
     */
    function startConnection(accessToken) {
        if (connection && connection.state === signalR.HubConnectionState.Connected) {
            return Promise.resolve(connection);
        }

        connection = new signalR.HubConnectionBuilder()
            .withUrl(CHAT_HUB_URL, {
                accessTokenFactory: () => accessToken
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        /**
         * Handles the "ReceiveMessage" event from the SignalR hub.
         * Updates the UI with the received message and marks the conversation as read if necessary.
         * @param {Object} messageDto - The message data transfer object.
         */
        connection.on("ReceiveMessage", (messageDto) => {
            console.log("Message received from hub:", messageDto);

            const currentUserId = Auth.getCurrentUserId();
            const isSender = messageDto.senderId === currentUserId;
            const isCurrentConversation = messageDto.conversationId === App.getCurrentConversationId();

            if (isCurrentConversation) {
                UI.updateConversationListOnNewMessage(messageDto, isSender, true);

                UI.renderSingleMessage(messageDto, currentUserId);
                UI.scrollToBottom();

                if (!isSender) {
                    Api.markConversationAsRead(messageDto.conversationId).catch(console.error);
                }
            } else {
                UI.updateConversationListOnNewMessage(messageDto, isSender, false);
            }
        });

        /**
         * Handles the "onreconnected" event from SignalR.
         * Rejoins the current conversation after reconnection.
         * @param {string} connectionId - The ID of the reconnected connection.
         */
        connection.onreconnected(async (connectionId) => {
            console.log(`SignalR reconnected with ID: ${connectionId}`);
            const currentConvId = App.getCurrentConversationId();
            if (currentConvId) {
                await joinConversation(currentConvId);
            }
        });

        return connection.start()
            .then(() => console.log('SignalR Connected to ChatHub.'))
            .catch(err => console.error('SignalR Connection Error: ', err));
    }

    /**
     * Sends a message to the specified conversation via the SignalR hub.
     * @param {string} conversationId - The ID of the conversation.
     * @param {string} messageText - The text content of the message.
     */
    async function sendMessage(conversationId, messageText) {
        if (connection?.state === signalR.HubConnectionState.Connected) {
            await connection.invoke("SendMessage", conversationId, messageText);
        } else {
            console.error('SignalR connection not established.');
        }
    }

    /**
     * Joins a conversation group via the SignalR hub.
     * @param {string} conversationId - The ID of the conversation to join.
     */
    async function joinConversation(conversationId) {
        if (connection?.state === signalR.HubConnectionState.Connected) {
            await connection.invoke("JoinConversation", conversationId);
        }
    }

    return {
        /**
         * Starts the SignalR connection to the ChatHub.
         * @param {string} accessToken - The access token for authentication.
         * @returns {Promise<signalR.HubConnection>} - Resolves with the SignalR connection instance.
         */
        startConnection,

        /**
         * Sends a message to the specified conversation via the SignalR hub.
         * @param {string} conversationId - The ID of the conversation.
         * @param {string} messageText - The text content of the message.
         */
        sendMessage,

        /**
         * Joins a conversation group via the SignalR hub.
         * @param {string} conversationId - The ID of the conversation to join.
         */
        joinConversation,
    };
})();