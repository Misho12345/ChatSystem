const ChatClient = (function () {
    const CHAT_HUB_URL = 'http://localhost:8002/chathub';
    let connection = null;

    function startConnection(accessToken) {
        if (connection && connection.state === signalR.HubConnectionState.Connected) {
            return;
        }

        connection = new signalR.HubConnectionBuilder()
            .withUrl(CHAT_HUB_URL, {
                accessTokenFactory: () => accessToken
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

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

    async function sendMessage(conversationId, messageText) {
        if (connection?.state === signalR.HubConnectionState.Connected) {
            await connection.invoke("SendMessage", conversationId, messageText);
        } else {
            console.error('SignalR connection not established.');
        }
    }

    async function joinConversation(conversationId) {
        if (connection?.state === signalR.HubConnectionState.Connected) {
            await connection.invoke("JoinConversation", conversationId);
        }
    }

    return {
        startConnection,
        sendMessage,
        joinConversation,
    };
})();