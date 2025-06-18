/**
 * Module for managing SignalR connection to the FriendshipHub and handling real-time notifications.
 */
const NotificationClient = (function () {
    const FRIENDSHIP_HUB_URL = '/friendshiphub';
    let connection = null;

    /**
     * Starts the SignalR connection to the FriendshipHub.
     * Reuses the existing connection if already established.
     * @param {string} accessToken - The access token for authentication.
     * @returns {Promise<signalR.HubConnection>} - Resolves with the SignalR connection instance.
     */
    function startConnection(accessToken) {
        if (connection && connection.state === signalR.HubConnectionState.Connected) {
            console.log('SignalR connection to FriendshipHub already established.');
            return Promise.resolve(connection);
        }

        connection = new signalR.HubConnectionBuilder()
            .withUrl(FRIENDSHIP_HUB_URL, {
                accessTokenFactory: () => accessToken
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        /**
         * Handles the "NewFriendRequest" event from the SignalR hub.
         * Updates the UI with the received friend request.
         * @param {Object} friendRequest - The friend request data.
         */
        connection.on("NewFriendRequest", (friendRequest) => {
            console.log("New friend request received:", friendRequest);
            UI.addFriendRequest(friendRequest);
        });

        /**
         * Handles the "FriendRequestProcessed" event from the SignalR hub.
         * Refreshes the friend request and friends list after a request is processed.
         */
        connection.on("FriendRequestProcessed", () => {
            console.log("A friend request was processed (accepted/declined). Refreshing lists.");
            loadInitialData();
        });

        /**
         * Handles the "FriendshipRemoved" event from the SignalR hub.
         * Refreshes the friends list after a friendship is removed.
         */
        connection.on("FriendshipRemoved", () => {
            console.log("A friend was removed. Refreshing lists.");
            loadInitialData();
        });

        return connection.start()
            .then(() => console.log('SignalR Connected to FriendshipHub.'))
            .catch(err => console.error('SignalR Connection Error to FriendshipHub: ', err));
    }

    return {
        /**
         * Starts the SignalR connection to the FriendshipHub.
         * @param {string} accessToken - The access token for authentication.
         * @returns {Promise<signalR.HubConnection>} - Resolves with the SignalR connection instance.
         */
        startConnection
    };
})();