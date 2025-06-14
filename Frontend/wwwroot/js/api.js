const Api = (function () {
    const USER_ACCOUNT_BASE_URL = 'http://localhost:8001/api';
    const CHAT_SERVICE_BASE_URL = 'http://localhost:8002/api';

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
        login: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/login`, 'POST', data, false),
        register: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/register`, 'POST', data, false),
        refreshToken: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/refresh`, 'POST', data, false),
        logout: (data) => request(`${USER_ACCOUNT_BASE_URL}/auth/logout`, 'POST', data, false),

        getCurrentUser: () => request(`${USER_ACCOUNT_BASE_URL}/users/me`),
        searchUsers: (query) => request(`${USER_ACCOUNT_BASE_URL}/users/search?query=${encodeURIComponent(query)}`),
        getFriends: () => request(`${USER_ACCOUNT_BASE_URL}/friends`),
        getPendingFriendRequests: () => request(`${USER_ACCOUNT_BASE_URL}/friends/requests/pending/incoming`),
        sendFriendRequest: (targetUserId) => request(`${USER_ACCOUNT_BASE_URL}/friends/request/${targetUserId}`, 'POST'),
        acceptFriendRequest: (requestId) => request(`${USER_ACCOUNT_BASE_URL}/friends/accept/${requestId}`, 'POST'),
        declineFriendRequest: (requestId) => request(`${USER_ACCOUNT_BASE_URL}/friends/decline/${requestId}`, 'POST'),

        getUserConversations: () => request(`${CHAT_SERVICE_BASE_URL}/conversations`),
        getMessagesForConversation: (conversationId, beforeTimestamp = null, limit = 20) => {
            let url = `${CHAT_SERVICE_BASE_URL}/conversations/${conversationId}/messages?limit=${limit}`;
            if (beforeTimestamp) {
                url += `&beforeTimestamp=${new Date(beforeTimestamp).toISOString()}`;
            }
            return request(url);
        },
        initiateConversation: (recipientId) => request(`${CHAT_SERVICE_BASE_URL}/conversations/initiate`, 'POST', {recipientId}),
    };
})();