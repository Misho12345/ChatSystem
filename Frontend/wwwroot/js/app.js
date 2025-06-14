document.addEventListener('DOMContentLoaded', async () => {
    Auth.init(); // Load tokens, check if on main.html and redirect if not logged in
    if (!Auth.isLoggedIn() && window.location.pathname.endsWith('main.html')) {
        window.location.href = 'index.html';
        return;
    }
    if (!window.location.pathname.endsWith('main.html')) return; // Only run on main.html

    UI.displayCurrentUser(); // Fetch and display current user info
    await loadInitialData();
    ChatClient.startConnection(Auth.getAccessToken());

    document.getElementById('logoutButton').addEventListener('click', Auth.logout);
    document.getElementById('searchButton').addEventListener('click', handleSearch);
    document.getElementById('searchInput').addEventListener('keypress', (e) => {
        if (e.key === 'Enter') handleSearch();
    });
    document.getElementById('sendMessageButton').addEventListener('click', handleSendMessage);
    document.getElementById('messageInput').addEventListener('keypress', (e) => {
        if (e.key === 'Enter') handleSendMessage();
    });

    document.getElementById('friendsList').addEventListener('click', handleFriendListClick);
    document.getElementById('friendRequestsList').addEventListener('click', handleFriendRequestAction);
    document.getElementById('searchResults').addEventListener('click', handleSearchResultClick);

    const messagesPanel = document.getElementById('messagesPanel');
    messagesPanel.addEventListener('scroll', UI.handleScrollForMessages);

});

let currentConversationId = null;
let currentRecipient = null;

async function loadInitialData() {
    try {
        const [friends, requests] = await Promise.all([
            Api.getFriends(),
            Api.getPendingFriendRequests()
        ]);
        
        UI.renderFriendsList(friends);
        UI.renderFriendRequests(requests);
    } catch (error) {
        console.error('Failed to load initial data:', error);
        if (error.status === 401) Auth.logout();
    }
}

async function handleSearch() {
    const query = document.getElementById('searchInput').value;
    if (!query.trim()) {
        UI.renderSearchResults();
        return;
    }
    try {
        const results = await Api.searchUsers(query);
        UI.renderSearchResults(results);
    } catch (error) {
        console.error('Search failed:', error);
        if (error.status === 401) Auth.logout();
    }
}

async function handleSendMessage() {
    const messageText = document.getElementById('messageInput').value;
    if (messageText.trim() && currentConversationId) {
        await ChatClient.sendMessage(currentConversationId, messageText);
        document.getElementById('messageInput').value = '';
    }
}

async function handleFriendListClick(event) {
    const target = event.target.closest('.list-group-item[data-friend-id]');
    if (target) {
        const friendId = target.dataset.friendId;
        const friendName = target.dataset.friendName;
        const friendTag = target.dataset.friendTag;

        currentRecipient = {id: friendId, name: friendName, tag: friendTag};
        UI.setActiveFriend(friendId);

        document.getElementById('chatHeader').textContent = `Chat with ${friendName} (${friendTag})`;
        document.getElementById('messagesPanel').innerHTML = '';
        document.getElementById('messageInputArea').classList.remove('d-none');
        document.getElementById('noChatSelected').classList.add('d-none');


        try {
            const conversation = await Api.initiateConversation(friendId);
            currentConversationId = conversation.id;
            await ChatClient.joinConversation(currentConversationId);

            const messages = await Api.getMessagesForConversation(currentConversationId, null, 20);
            UI.renderMessages(messages, Auth.getCurrentUserId());
            UI.scrollToBottom();
        } catch (error) {
            console.error('Failed to start chat:', error);
            if (error.status === 401) Auth.logout();
        }
    }
}

async function handleFriendRequestAction(event) {
    const button = event.target.closest('button[data-request-id]');
    if (!button) return;

    const requestId = button.dataset.requestId;
    const action = button.dataset.action;

    try {
        if (action === 'accept') {
            await Api.acceptFriendRequest(requestId);
            alert('Friend request accepted!');
        } else if (action === 'decline') {
            await Api.declineFriendRequest(requestId);
            alert('Friend request declined.');
        }
        await loadInitialData();
    } catch (error) {
        console.error(`Failed to ${action} friend request:`, error);
        if (error.status === 401) Auth.logout();
    }
}

async function handleSearchResultClick(event) {
    const button = event.target.closest('button[data-user-id]');
    if (!button) return;

    const targetUserId = button.dataset.userId;
    try {
        await Api.sendFriendRequest(targetUserId);
        alert('Friend request sent!');
    } catch (error) {
        console.error('Failed to send friend request:', error);
        alert(error.message || 'Failed to send friend request.');
        if (error.status === 401) Auth.logout();
    }
}

App = {
    getCurrentConversationId: () => currentConversationId,
    getCurrentRecipient: () => currentRecipient
};