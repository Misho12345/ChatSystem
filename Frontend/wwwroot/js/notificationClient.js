const NotificationClient = (function () {
    const FRIENDSHIP_HUB_URL = 'http://localhost:8001/friendshiphub';
    let connection = null;

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

        connection.on("NewFriendRequest", (friendRequest) => {
            console.log("New friend request received:", friendRequest);
            UI.addFriendRequest(friendRequest);
        });

        connection.on("FriendRequestProcessed", () => {
            console.log("A friend request was processed (accepted/declined). Refreshing lists.");
            loadInitialData();
        });

        connection.on("FriendshipRemoved", () => {
            console.log("A friend was removed. Refreshing lists.");
            loadInitialData();
        });

        return connection.start()
            .then(() => console.log('SignalR Connected to FriendshipHub.'))
            .catch(err => console.error('SignalR Connection Error to FriendshipHub: ', err));
    }

    return {startConnection};
})();