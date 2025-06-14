const UI = (function () {
    let oldestMessageTimestamp = null;
    let isLoadingMessages = false;

    function addFriendRequest(req) {
        const listElement = document.getElementById('friendRequestsList');
        const countElement = document.getElementById('friendRequestCount');

        const noRequestsEl = listElement.querySelector('li');
        if (noRequestsEl && noRequestsEl.textContent.includes("No pending requests")) {
            listElement.innerHTML = '';
        }

        const li = document.createElement('li');
        li.className = 'list-group-item d-flex justify-content-between align-items-center';
        li.innerHTML = `
            <span>${req.requesterName} (${req.requesterTag})</span>
            <div>
                <button class="btn btn-sm btn-success me-1" data-request-id="${req.id}" data-action="accept"><i class="fas fa-check"></i></button>
                <button class="btn btn-sm btn-danger" data-request-id="${req.id}" data-action="decline"><i class="fas fa-times"></i></button>
            </div>
        `;
        listElement.prepend(li);
        countElement.textContent = parseInt(countElement.textContent, 10) + 1;
    }

    function renderFriendsList(friends) {
        const listElement = document.getElementById('friendsList');
        const countElement = document.getElementById('friendsCount');
        listElement.innerHTML = '';
        if (friends && friends.length > 0) {
            friends.forEach(friend => {
                const li = document.createElement('li');
                li.className = 'list-group-item list-group-item-action';
                li.style.cursor = 'pointer';
                li.dataset.friendId = friend.id;
                li.dataset.friendName = friend.name;
                li.dataset.friendTag = friend.tag;
                li.innerHTML = `
                    <div>
                        <strong>${friend.name}</strong> (${friend.tag})
                        </div>
                `;
                listElement.appendChild(li);
            });
            countElement.textContent = friends.length;
        } else {
            listElement.innerHTML = '<li class="list-group-item">No friends yet.</li>';
            countElement.textContent = 0;
        }
    }

    function renderFriendRequests(requests) {
        const listElement = document.getElementById('friendRequestsList');
        const countElement = document.getElementById('friendRequestCount');
        listElement.innerHTML = '';
        if (requests && requests.length > 0) {
            requests.forEach(req => {
                const li = document.createElement('li');
                li.className = 'list-group-item d-flex justify-content-between align-items-center';
                li.innerHTML = `
                    <span>${req.requesterName} (${req.requesterTag})</span>
                    <div>
                        <button class="btn btn-sm btn-success me-1" data-request-id="${req.id}" data-action="accept"><i class="fas fa-check"></i></button>
                        <button class="btn btn-sm btn-danger" data-request-id="${req.id}" data-action="decline"><i class="fas fa-times"></i></button>
                    </div>
                `;
                listElement.appendChild(li);
            });
            countElement.textContent = requests.length;
        } else {
            listElement.innerHTML = '<li class="list-group-item">No pending requests.</li>';
            countElement.textContent = 0;
        }
    }

    function renderSearchResults(users) {
        const listElement = document.getElementById('searchResults');
        listElement.innerHTML = '';
        if (users && users.length > 0) {
            users.forEach(user => {
                if (user.id === Auth.getCurrentUserId()) return;

                const li = document.createElement('li');
                li.className = 'list-group-item d-flex justify-content-between align-items-center';
                li.innerHTML = `
                    <span>${user.name} (${user.tag})</span>
                    <button class="btn btn-sm btn-info" data-user-id="${user.id}"><i class="fas fa-user-plus"></i> Add</button>
                `;
                listElement.appendChild(li);
            });
        } else {
            listElement.innerHTML = '<li class="list-group-item">No users found.</li>';
        }
    }

    function createMessageElement(messageDto, currentUserId) {
        const div = document.createElement('div');
        const isSender = messageDto.senderId === currentUserId;
        div.className = `message-bubble mb-2 p-2 rounded ${isSender ? 'sent bg-primary text-white ms-auto' : 'received bg-light text-dark me-auto'}`;
        div.style.maxWidth = '70%';
        div.style.overflowWrap = 'break-word';

        const senderTag = isSender ? 'You' : (messageDto.senderTag || 'Them');
        div.innerHTML = `
            <small class="fw-bold">${senderTag}</small>
            <div>${escapeHtml(messageDto.text)}</div>
            <small class="message-timestamp text-muted d-block text-end">${new Date(messageDto.timestamp).toLocaleTimeString()}</small>
        `;
        div.dataset.timestamp = messageDto.timestamp;
        return div;
    }

    function renderMessages(messages, currentUserId) {
        const panel = document.getElementById('messagesPanel');
        panel.innerHTML = '';
        if (messages && messages.length > 0) {
            messages.forEach(msg => {
                panel.appendChild(createMessageElement(msg, currentUserId));
            });
            oldestMessageTimestamp = messages[0].timestamp;
        } else {
            panel.innerHTML = '<p class="text-center text-muted">No messages yet. Start the conversation!</p>';
            oldestMessageTimestamp = null;
        }
    }

    function renderSingleMessage(messageDto, currentUserId) {
        const panel = document.getElementById('messagesPanel');
        const placeholder = panel.querySelector('p.text-center.text-muted');
        if (placeholder) placeholder.remove();

        panel.appendChild(createMessageElement(messageDto, currentUserId));
        if (!oldestMessageTimestamp || new Date(messageDto.timestamp) < new Date(oldestMessageTimestamp)) {
            oldestMessageTimestamp = messageDto.timestamp;
        }
    }

    function prependMessages(messages, currentUserId) {
        const panel = document.getElementById('messagesPanel');
        const originalScrollHeight = panel.scrollHeight;
        const originalScrollTop = panel.scrollTop;

        if (messages && messages.length > 0) {
            messages.slice().reverse().forEach(msg => {
                panel.insertBefore(createMessageElement(msg, currentUserId), panel.firstChild);
            });
            oldestMessageTimestamp = messages[0].timestamp;

            panel.scrollTop = originalScrollTop + (panel.scrollHeight - originalScrollHeight);
        }
        isLoadingMessages = false;
    }

    function scrollToBottom() {
        const panel = document.getElementById('messagesPanel');
        panel.scrollTop = panel.scrollHeight;
    }

    async function handleScrollForMessages() {
        const panel = document.getElementById('messagesPanel');
        if (panel.scrollTop === 0 && !isLoadingMessages && currentConversationId && oldestMessageTimestamp) {
            isLoadingMessages = true;
            console.log(`Fetching messages before ${oldestMessageTimestamp} for ${currentConversationId}`);
            try {
                const olderMessages = await Api.getMessagesForConversation(currentConversationId, oldestMessageTimestamp, 20);
                if (olderMessages && olderMessages.length > 0) {
                    prependMessages(olderMessages, Auth.getCurrentUserId());
                } else {
                    console.log('No more older messages to load.');
                    oldestMessageTimestamp = null;
                }
            } catch (error) {
                console.error('Failed to load older messages:', error);
                if (error.status === 401) Auth.logout();
            } finally {
                isLoadingMessages = false;
            }
        }
    }

    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    function setActiveFriend(friendId) {
        document.querySelectorAll('#friendsList .list-group-item').forEach(item => {
            item.classList.remove('active');
        });
        const activeItem = document.querySelector(`#friendsList .list-group-item[data-friend-id="${friendId}"]`);
        if (activeItem) {
            activeItem.classList.add('active');
        }
        oldestMessageTimestamp = null;
        isLoadingMessages = false;
    }

    async function displayCurrentUser() {
        const display = document.getElementById('currentUserDisplay');
        if (Auth.isLoggedIn()) {
            try {
                const user = await Api.getCurrentUser();
                Auth.setCurrentUserId(user.id);
                if (display) display.textContent = `Logged in as: ${user.name} (${user.tag})`;
            } catch (error) {
                console.error("Failed to fetch current user details", error);
                if (display) display.textContent = 'Logged in';
                if (error.status === 401) Auth.logout();
            }
        } else {
            if (display) display.textContent = 'Not logged in';
        }
    }

    Auth.setCurrentUserId = function (id) {
        this.currentUserId = id;
    };
    Auth.getCurrentUserId = function () {
        return this.currentUserId;
    };


    return {
        renderFriendsList,
        renderFriendRequests,
        renderSearchResults,
        renderMessages,
        renderSingleMessage,
        prependMessages,
        scrollToBottom,
        handleScrollForMessages,
        setActiveFriend,
        displayCurrentUser,
        addFriendRequest
    };
})();