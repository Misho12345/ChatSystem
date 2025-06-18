/**
 * Module for interacting with the API endpoints of the User Account and Chat Service.
 */
const Api = (function () {
    const USER_ACCOUNT_BASE_URL = '/api/useraccount';
    const CHAT_SERVICE_BASE_URL = '/api/chat';

    /**
     * Makes an HTTP request to the specified URL with the given method, body, and authentication requirements.
     * @param {string} url - The URL to send the request to.
     * @param {string} [method='GET'] - The HTTP method to use (e.g., 'GET', 'POST').
     * @param {Object|null} [body=null] - The request body to send, if applicable.
     * @param {boolean} [requiresAuth=true] - Whether the request requires authentication.
     * @returns {Promise<Object|null>} - The response data or null for no content.
     * @throws {Error} - Throws an error if the request fails or authentication is invalid.
     */
    async function request(url, method = 'GET', body = null, requiresAuth = true) {
        const headers = {'Content-Type': 'application/json'};
        if (requiresAuth) {
            let token = Auth.getAccessToken();
            if (!token) {
                console.error('No access token available for authenticated request.');
                Auth.logout();
                throw new Error('Not authenticated');
            }
            headers['Authorization'] = `Bearer ${token}`;
        }

        const config = {
            method: method,
            headers: headers,
        };

        if (body) {
            config.body = JSON.stringify(body);
        }

        try {
            let response = await fetch(url, config);

            if (response.status === 401 && requiresAuth) {
                console.log('Access token expired or invalid. Attempting refresh...');
                const refreshed = await Auth.attemptRefresh();
                if (refreshed) {
                    headers['Authorization'] = `Bearer ${Auth.getAccessToken()}`;
                    response = await fetch(url, config);
                } else {
                    Auth.logout();
                    throw new Error('Session expired. Please login again.');
                }
            }

            if (!response.ok) {
                const errorData = await response.json().catch(() => ({message: response.statusText}));
                throw {status: response.status, message: errorData.message || errorData.title || 'API request failed'};
            }
            if (response.status === 204 || response.headers.get("content-length") === "0") {
                return null;
            }
            return await response.json();
        } catch (error) {
            console.error(`API Error(${method} ${url}):`, error.status, error.message);
            throw error;
        }
    }

    return {
        /**
         * Sends a login request to the User Account service.
         * @param {Object} data - The login credentials.
         * @returns {Promise<Object>} - The response data.
         */
        login: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/login`, 'POST', data, false),

        /**
         * Sends a registration request to the User Account service.
         * @param {Object} data - The registration details.
         * @returns {Promise<Object>} - The response data.
         */
        register: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/register`, 'POST', data, false),

        /**
         * Sends a token refresh request to the User Account service.
         * @param {Object} data - The refresh token details.
         * @returns {Promise<Object>} - The response data.
         */
        refreshToken: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/refresh`, 'POST', data, false),

        /**
         * Sends a logout request to the User Account service.
         * @param {Object} data - The logout details.
         * @returns {Promise<Object>} - The response data.
         */
        logout: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/logout`, 'POST', data, false),

        /**
         * Retrieves the current user's information.
         * @returns {Promise<Object>} - The user data.
         */
        getCurrentUser: () => request(`${USER_ACCOUNT_BASE_URL}/users/me`),

        /**
         * Searches for users based on a query string.
         * @param {string} query - The search query.
         * @returns {Promise<Object[]>} - The list of matching users.
         */
        searchUsers: (query) => request(`${USER_ACCOUNT_BASE_URL}/users/search?query=${encodeURIComponent(query)}`),

        /**
         * Retrieves the current user's friends.
         * @returns {Promise<Object[]>} - The list of friends.
         */
        getFriends: () => request(`${USER_ACCOUNT_BASE_URL}/friends`),

        /**
         * Retrieves the pending friend requests for the current user.
         * @returns {Promise<Object[]>} - The list of pending friend requests.
         */
        getPendingFriendRequests: () => request(`${USER_ACCOUNT_BASE_URL}/friends/requests/pending/incoming`),

        /**
         * Sends a friend request to a target user.
         * @param {string} targetUserId - The ID of the target user.
         * @returns {Promise<Object>} - The response data.
         */
        sendFriendRequest: (targetUserId) => request(`${USER_ACCOUNT_BASE_URL}/friends/request/${targetUserId}`, 'POST'),

        /**
         * Accepts a pending friend request.
         * @param {string} requestId - The ID of the friend request.
         * @returns {Promise<Object>} - The response data.
         */
        acceptFriendRequest: (requestId) => request(`${USER_ACCOUNT_BASE_URL}/friends/accept/${requestId}`, 'POST'),

        /**
         * Declines a pending friend request.
         * @param {string} requestId - The ID of the friend request.
         * @returns {Promise<Object>} - The response data.
         */
        declineFriendRequest: (requestId) => request(`${USER_ACCOUNT_BASE_URL}/friends/decline/${requestId}`, 'POST'),

        /**
         * Retrieves the user's conversations.
         * @returns {Promise<Object[]>} - The list of conversations.
         */
        getUserConversations: () => request(`${CHAT_SERVICE_BASE_URL}/conversations`),

        /**
         * Retrieves messages for a specific conversation.
         * @param {string} conversationId - The ID of the conversation.
         * @param {string|null} [beforeTimestamp=null] - The timestamp to retrieve messages before.
         * @param {number} [limit=20] - The maximum number of messages to retrieve.
         * @returns {Promise<Object[]>} - The list of messages.
         */
        getMessagesForConversation: (conversationId, beforeTimestamp = null, limit = 20) => {
            let url = `${CHAT_SERVICE_BASE_URL}/conversations/${conversationId}/messages?limit=${limit}`;
            if (beforeTimestamp) {
                url += `&beforeTimestamp=${new Date(beforeTimestamp).toISOString()}`;
            }
            return request(url);
        },

        /**
         * Initiates a conversation with a recipient.
         * @param {string} recipientId - The ID of the recipient.
         * @returns {Promise<Object>} - The response data.
         */
        initiateConversation: (recipientId) => request(`${CHAT_SERVICE_BASE_URL}/conversations/initiate`, 'POST', {recipientId}),

        /**
         * Marks a conversation as read.
         * @param {string} conversationId - The ID of the conversation.
         * @returns {Promise<Object>} - The response data.
         */
        markConversationAsRead: (conversationId) => request(`${CHAT_SERVICE_BASE_URL}/conversations/${conversationId}/mark-as-read`, 'POST'),
    };
})();