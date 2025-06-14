const ChatClient = (function () {
    const CHAT_HUB_URL = 'http://localhost:8002/chathub';
    let connection = null;

    function startConnection(accessToken) {
        if (connection && connection.state === signalR.HubConnectionState.Connected) {
            console.log('SignalR connection already established.');
            return Promise.resolve(connection);
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
            UI.renderSingleMessage(messageDto, Auth.getCurrentUserId());
            UI.scrollToBottom();
        });

        connection.on("LoadEarlierMessages", (conversationId, messages) => {
            console.log(`Earlier messages for ${conversationId} received:`, messages);
            UI.prependMessages(messages, Auth.getCurrentUserId());
        });

        connection.onreconnected(connectionId => {
            console.log(`SignalR reconnected with ID: ${connectionId}`);
            const currentConvId = App.getCurrentConversationId();
            if (currentConvId) {
                joinConversation(currentConvId);
            }
        });


        return connection.start()
            .then(() => console.log('SignalR Connected to ChatHub.'))
            .catch(err => {
                console.error('SignalR Connection Error: ', err);
                if (err.statusCode === 401 || (err.message && err.message.includes("401"))) {
                    console.log("SignalR connection unauthorized, attempting token refresh...");
                    Auth.attemptRefresh().then(refreshed => {
                        if (refreshed) {
                            console.log("Token refreshed, retrying SignalR connection...");
                            startConnection(Auth.getAccessToken());
                        } else {
                            Auth.logout();
                        }
                    });
                }
            });
    }

    async function sendMessage(conversationId, messageText) {
        if (connection && connection.state === signalR.HubConnectionState.Connected) {
            try {
                await connection.invoke("SendMessage", conversationId, messageText);
            } catch (err) {
                console.error('Error sending message via SignalR:', err);
            }
        } else {
            console.error('SignalR connection not established.');
        }
    }

    async function joinConversation(conversationId) {
        if (connection && connection.state === signalR.HubConnectionState.Connected) {
            try {
                await connection.invoke("JoinConversation", conversationId);
                console.log(`Joined SignalR group for conversation: ${conversationId}`);
            } catch (err) {
                console.error(`Error joining SignalR group ${conversationId}:`, err);
            }
        }
    }

    async function leaveConversation(conversationId) {
        if (connection && connection.state === signalR.HubConnectionState.Connected && conversationId) {
            try {
                await connection.invoke("LeaveConversation", conversationId);
                console.log(`Left SignalR group for conversation: ${conversationId}`);
            } catch (err) {
                console.error(`Error leaving SignalR group ${conversationId}:`, err);
            }
        }
    }

    return {
        startConnection,
        sendMessage,
        joinConversation,
        leaveConversation,
        getConnection: () => connection
    };
})();