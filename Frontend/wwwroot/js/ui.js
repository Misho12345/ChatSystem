const UI = (function () {
    let oldestMessageTimestamp = null;
    let isLoadingMessages = false;

    function createSeparatorElement() {
        const separator = document.createElement('div');
        separator.className = 'separator-container';
        separator.innerHTML = `
            <div class="separator-line"></div>
            <span class="separator-text">New Messages</span>
            <div class="separator-line"></div>
        `;
        return separator;
    }

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

    function renderConversationsList(conversations, friends) {
        const listElement = document.getElementById('friendsList');
        const countElement = document.getElementById('friendsCount');
        listElement.innerHTML = '';

        if (friends && friends.length > 0) {
            const conversationsMap = new Map(conversations.map(conv => {
                const otherParticipantId = conv.participantIds.find(id => id !== Auth.getCurrentUserId());
                return [otherParticipantId, conv];
            }));

            friends.sort((a, b) => {
                const convA = conversationsMap.get(a.id);
                const convB = conversationsMap.get(b.id);
                const timeA = convA ? new Date(convA.updatedAt).getTime() : 0;
                const timeB = convB ? new Date(convB.updatedAt).getTime() : 0;
                return timeB - timeA;
            });

            friends.forEach(friendInfo => {
                const conv = conversationsMap.get(friendInfo.id);

                const li = document.createElement('li');
                li.className = 'list-group-item list-group-item-action';
                li.style.cursor = 'pointer';

                li.dataset.conversationId = conv?.id ?? 'null';
                li.dataset.friendId = friendInfo.id;
                li.dataset.friendName = friendInfo.name;
                li.dataset.friendTag = friendInfo.tag;

                const unreadCount = conv?.unreadCount ?? 0;
                const lastMessageText = escapeHtml(conv?.lastMessage?.text ?? 'Click to start a conversation.');

                li.dataset.unreadCount = unreadCount;

                const unreadBadge = unreadCount > 0
                    ? `<span class="badge bg-danger rounded-pill ms-2 unread-badge">${unreadCount}</span>`
                    : '';

                li.innerHTML = `
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <strong>${friendInfo.name}</strong> (${friendInfo.tag})
                            <div class="text-muted small last-message">${lastMessageText}</div>
                        </div>
                        ${unreadBadge}
                    </div>
                `;
                listElement.appendChild(li);
            });
            countElement.textContent = friends.length;
        } else {
            listElement.innerHTML = '<li class="list-group-item">No friends yet. Find users to add!</li>';
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

    function renderMessages(messages, currentUserId, unreadCount = 0) {
        const panel = document.getElementById('messagesPanel');
        panel.innerHTML = '';

        if (messages && messages.length > 0) {
            messages.slice().reverse().forEach(msg => {
                panel.appendChild(createMessageElement(msg, currentUserId));
            });

            if (unreadCount > 0) {
                const messageElements = panel.querySelectorAll('.message-bubble');
                if (unreadCount <= messageElements.length) {
                    const firstUnreadNode = messageElements[messageElements.length - unreadCount];
                    if (firstUnreadNode) {
                        panel.insertBefore(createSeparatorElement(), firstUnreadNode);
                    }
                } else {
                    panel.prepend(createSeparatorElement());
                }
            }
            oldestMessageTimestamp = messages[messages.length - 1].timestamp;
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
            oldestMessageTimestamp = messages[messages.length - 1].timestamp;

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
        if (panel.scrollTop === 0 && !isLoadingMessages && App.getCurrentConversationId() && oldestMessageTimestamp) {
            isLoadingMessages = true;
            try {
                const olderMessages = await Api.getMessagesForConversation(App.getCurrentConversationId(), oldestMessageTimestamp, 20);
                if (olderMessages && olderMessages.length > 0) {
                    prependMessages(olderMessages, Auth.getCurrentUserId());
                } else {
                    oldestMessageTimestamp = null;
                }
            } catch (error) {
                console.error('Failed to load older messages:', error);
            } finally {
                isLoadingMessages = false;
            }
        }
    }

    function escapeHtml(unsafe) {
        if (typeof unsafe !== 'string') {
            return '';
        }
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    function setActiveConversation(conversationId) {
        document.querySelectorAll('#friendsList .list-group-item').forEach(item => {
            item.classList.remove('active');
        });
        const activeItem = document.querySelector(`#friendsList .list-group-item[data-conversation-id="${conversationId}"]`);
        if (activeItem) {
            activeItem.classList.add('active');
        }
        oldestMessageTimestamp = null;
        isLoadingMessages = false;
    }

    function updateConversationListOnNewMessage(messageDto, isSender, isChatOpen) {
        const listElement = document.getElementById('friendsList');
        let conversationElement = listElement.querySelector(`[data-conversation-id="${messageDto.conversationId}"]`);

        if (!conversationElement && !isSender) {
            const friendId = messageDto.senderId;
            conversationElement = listElement.querySelector(`[data-friend-id="${friendId}"]`);
        }

        if (conversationElement) {
            if (conversationElement.dataset.conversationId === 'null') {
                conversationElement.dataset.conversationId = messageDto.conversationId;
            }

            const lastMessageEl = conversationElement.querySelector('.last-message');
            const lastMessageText = isSender ? `You: ${messageDto.text}` : messageDto.text;
            if (lastMessageEl) lastMessageEl.textContent = lastMessageText;

            listElement.prepend(conversationElement);

            if (!isSender && !isChatOpen) {
                let unreadCount = parseInt(conversationElement.dataset.unreadCount, 10) + 1;
                conversationElement.dataset.unreadCount = unreadCount;

                let badge = conversationElement.querySelector('.unread-badge');
                if (!badge) {
                    badge = document.createElement('span');
                    badge.className = 'badge bg-danger rounded-pill ms-2 unread-badge';
                    conversationElement.querySelector('.d-flex').appendChild(badge);
                }
                badge.textContent = unreadCount;
            }
        }
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
        renderConversationsList,
        renderFriendRequests,
        renderSearchResults,
        renderMessages,
        renderSingleMessage,
        prependMessages,
        scrollToBottom,
        handleScrollForMessages,
        setActiveFriend: setActiveConversation,
        displayCurrentUser,
        addFriendRequest,
        updateConversationListOnNewMessage
    };
})();