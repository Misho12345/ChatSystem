let currentConversationId = null;

/**
 * Initializes the application once the DOM content is fully loaded.
 * Sets up authentication, UI elements, and event listeners for user interactions.
 */
document.addEventListener('DOMContentLoaded', async () => {
    Auth.init();
    if (!Auth.isLoggedIn() && window.location.pathname.endsWith('main.html')) {
        window.location.href = 'index.html';
        return;
    }
    if (!window.location.pathname.endsWith('main.html')) return;

    await UI.displayCurrentUser();
    await loadInitialData();
    ChatClient.startConnection(Auth.getAccessToken());
    NotificationClient.startConnection(Auth.getAccessToken());

    document.getElementById('logoutButton').addEventListener('click', Auth.logout);
    document.getElementById('searchButton').addEventListener('click', handleSearch);
    document.getElementById('searchInput').addEventListener('keypress', (e) => {
        if (e.key === 'Enter') handleSearch();
    });
    document.getElementById('sendMessageButton').addEventListener('click', handleSendMessage);
    document.getElementById('messageInput').addEventListener('keypress', (e) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSendMessage();
        }
    });

    document.getElementById('conversationsList').addEventListener('click', handleConversationClick);
    document.getElementById('friendRequestsList').addEventListener('click', handleFriendRequestAction);
    document.getElementById('searchResults').addEventListener('click', handleSearchResultClick);

    const messagesPanel = document.getElementById('messagesPanel');
    messagesPanel.addEventListener('scroll', UI.handleScrollForMessages);
});

/**
 * Loads initial data for the application, including conversations, friends, and friend requests.
 * Handles authentication errors and updates the UI accordingly.
 */
async function loadInitialData() {
    try {
        const [conversations, friends, requests] = await Promise.all([
            Api.getUserConversations(),
            Api.getFriends(),
            Api.getPendingFriendRequests()
        ]);
        friendsData = friends;
        UI.renderConversationsList(conversations, friendsData);
        UI.renderFriendRequests(requests);
    } catch (error) {
        console.error('Failed to load initial data:', error);
        if (error.status === 401) Auth.logout();
    }
}

/**
 * Handles the search functionality for users.
 * Sends a search query to the API and updates the UI with the results.
 */
async function handleSearch() {
    const query = document.getElementById('searchInput').value;
    if (!query.trim()) {
        UI.renderSearchResults([]);
        return;
    }
    try {
        const results = await Api.searchUsers(query);
        UI.renderSearchResults(results);
    } catch (error) {
        console.error('Search failed:', error);
    }
}

/**
 * Sends a message in the currently active conversation.
 * Clears the input field after sending the message.
 */
async function handleSendMessage() {
    const messageText = document.getElementById('messageInput').value;
    if (messageText.trim() && currentConversationId) {
        try {
            await ChatClient.sendMessage(currentConversationId, messageText);
            document.getElementById('messageInput').value = '';
        } catch (error) {
            console.error("Failed to send message:", error);
        }
    }
}

/**
 * Handles the click event on a conversation item.
 * Sets the active conversation, updates the UI, and loads messages for the conversation.
 */
async function handleConversationClick(event) {
    const target = event.target.closest('.list-group-item[data-conversation-id]');
    if (target) {
        const friendId = target.dataset.friendId;
        const friendName = target.dataset.friendName;
        const friendTag = target.dataset.friendTag;
        const unreadCount = parseInt(target.dataset.unreadCount, 10);
        let conversationId = target.dataset.conversationId;

        UI.setActiveConversation(conversationId);
        document.getElementById('chatHeader').textContent = `Chat with ${friendName} (${friendTag})`;
        document.getElementById('messagesPanel').innerHTML = '';
        document.getElementById('messageInputArea').classList.remove('d-none');

        try {
            if (conversationId === "null" || !conversationId) {
                const conversation = await Api.initiateConversation(friendId);
                conversationId = conversation.id;
                target.dataset.conversationId = conversationId;
            }

            currentConversationId = conversationId;

            if (unreadCount > 0) {
                await Api.markConversationAsRead(currentConversationId);
                target.dataset.unreadCount = '0';
                target.querySelector('.unread-badge')?.remove();
            }

            const messages = await Api.getMessagesForConversation(currentConversationId, null, 50);
            UI.renderMessages(messages, Auth.getCurrentUserId(), unreadCount);
            UI.scrollToBottom();

        } catch (error) {
            console.error('Failed to start chat:', error);
        }
    }
}

/**
 * Handles actions on friend requests, such as accepting or declining.
 * Updates the UI after performing the action.
 */
async function handleFriendRequestAction(event) {
    const button = event.target.closest('button[data-request-id]');
    if (!button) return;

    const requestId = button.dataset.requestId;
    const action = button.dataset.action;

    try {
        if (action === 'accept') {
            await Api.acceptFriendRequest(requestId);
        } else if (action === 'decline') {
            await Api.declineFriendRequest(requestId);
        }
        await loadInitialData();
    } catch (error) {
        console.error(`Failed to ${action} friend request:`, error);
    }
}

/**
 * Handles the click event on a search result item.
 * Sends a friend request to the selected user.
 */
async function handleSearchResultClick(event) {
    const button = event.target.closest('button[data-user-id]');
    if (!button) return;

    const targetUserId = button.dataset.userId;
    try {
        await Api.sendFriendRequest(targetUserId);
        button.textContent = 'Sent';
        button.disabled = true;
    } catch (error) {
        console.error('Failed to send friend request:', error);
        alert(error.message || 'Failed to send friend request.');
    }
}

/**
 * Provides access to application-level functions and data.
 */
window.App = {
    /**
     * Retrieves the ID of the currently active conversation.
     * @returns {string|null} The current conversation ID.
     */
    getCurrentConversationId: () => currentConversationId,

    /**
     * Loads initial data for the application.
     */
    loadInitialData,
};