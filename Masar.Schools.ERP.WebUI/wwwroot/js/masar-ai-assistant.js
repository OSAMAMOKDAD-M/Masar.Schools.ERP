/**
 * مسار الذكي - المساعد الرقمي (Masar AI Assistant)
 * Version: 1.0.1
 * Author: Masar Schools ERP System
 */

(function() {
    'use strict';

    // Global variables
    let chatHistory = [];
    let isChatOpen = false;
    let isTyping = false;
    const storageKey = 'masar_ai_chat_history';
    const apiEndpoint = '/api/ai-assistant/chat';
    const historyEndpoint = '/api/ai-assistant/history';

    // DOM Elements
    const widget = document.getElementById('masar-ai-widget');
    const toggleBtn = document.getElementById('masar-ai-toggle');
    const chatContainer = document.getElementById('masar-ai-chat');
    const closeBtn = document.getElementById('masar-ai-close');
    const clearBtn = document.getElementById('masar-ai-clear');
    const messagesContainer = document.getElementById('masar-ai-messages');
    const inputField = document.getElementById('masar-ai-input');
    const sendBtn = document.getElementById('masar-ai-send');

    // Initialize
    function init() {
        console.log('Masar AI Assistant: Initializing...');

        if (!widget) {
            console.error('Masar AI Assistant: Widget element not found');
            return;
        }

        if (!toggleBtn) {
            console.error('Masar AI Assistant: Toggle button not found');
            return;
        }

        if (!chatContainer) {
            console.error('Masar AI Assistant: Chat container not found');
            return;
        }

        console.log('Masar AI Assistant: All elements found, setting up event listeners');

        // Load chat history from localStorage
        loadChatHistory();

        // Event listeners
        toggleBtn.addEventListener('click', function(e) {
            console.log('Masar AI Assistant: Toggle button clicked');
            toggleChat();
        });
        closeBtn.addEventListener('click', closeChat);
        clearBtn.addEventListener('click', clearHistory);
        sendBtn.addEventListener('click', sendMessage);
        inputField.addEventListener('keypress', handleKeyPress);
        inputField.addEventListener('input', autoResize);

        // Load chat history from server
        loadServerHistory();

        console.log('Masar AI Assistant: Initialization complete');
    }

    // Toggle chat open/close
    function toggleChat() {
        console.log('Masar AI Assistant: Toggling chat, current state:', isChatOpen);
        isChatOpen = !isChatOpen;

        if (chatContainer) {
            chatContainer.classList.toggle('d-none', !isChatOpen);
            console.log('Masar AI Assistant: Chat container classes after toggle:', chatContainer.className);
        } else {
            console.error('Masar AI Assistant: Chat container is null');
        }

        if (isChatOpen) {
            if (inputField) inputField.focus();
            scrollToBottom();
        }
    }

    // Close chat
    function closeChat() {
        isChatOpen = false;
        chatContainer.classList.add('d-none');
    }

    // Load chat history from localStorage
    function loadChatHistory() {
        try {
            const stored = localStorage.getItem(storageKey);
            if (stored) {
                chatHistory = JSON.parse(stored);
            }
        } catch (e) {
            console.error('Error loading chat history:', e);
            chatHistory = [];
        }
    }

    // Save chat history to localStorage
    function saveChatHistory() {
        try {
            localStorage.setItem(storageKey, JSON.stringify(chatHistory));
        } catch (e) {
            console.error('Error saving chat history:', e);
        }
    }

    // Load chat history from server
    async function loadServerHistory() {
        try {
            const response = await fetch(`${historyEndpoint}?limit=10`);
            if (response.ok) {
                const serverHistory = await response.json();
                if (serverHistory && serverHistory.length > 0) {
                    // Clear default welcome message
                    messagesContainer.innerHTML = '';
                    // Render server history
                    serverHistory.forEach(msg => renderMessage(msg.sender, msg.message, false));
                    scrollToBottom();
                }
            }
        } catch (e) {
            console.error('Error loading server history:', e);
        }
    }

    // Clear chat history
    async function clearHistory() {
        if (!confirm('هل أنت متأكد من مسح سجل المحادثات؟')) return;

        try {
            const response = await fetch(historyEndpoint, {
                method: 'DELETE'
            });

            if (response.ok) {
                chatHistory = [];
                saveChatHistory();
                messagesContainer.innerHTML = '';
                addWelcomeMessage();
            }
        } catch (e) {
            console.error('Error clearing history:', e);
            alert('حدث خطأ أثناء مسح السجل');
        }
    }

    // Add welcome message
    function addWelcomeMessage() {
        const welcomeMessage = `
            أهلاً بك! أنا مسار الذكي، المساعد الرقمي لنظام مَسَار للمدارس.
            <br><br>
            يمكنني مساعدتك في:
            <ul>
                <li>🏢 إدارة المؤسسات والشعارات</li>
                <li>🎓 شؤون الطلاب والكنترول</li>
                <li>💰 الفواتير والمالية ZATCA</li>
                <li>📊 مؤشر الخطر للطلاب</li>
                <li>👥 إدارة الصلاحيات</li>
                <li>📅 الحضور والغياب</li>
            </ul>
            <br>
            كيف يمكنني مساعدتك اليوم؟
        `;
        renderMessage('assistant', welcomeMessage, false);
    }

    // Handle key press
    function handleKeyPress(e) {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            sendMessage();
        }
    }

    // Auto resize textarea
    function autoResize() {
        inputField.style.height = 'auto';
        inputField.style.height = Math.min(inputField.scrollHeight, 100) + 'px';
    }

    // Send message
    async function sendMessage() {
        const message = inputField.value.trim();
        if (!message || isTyping) return;

        // Clear input
        inputField.value = '';
        inputField.style.height = 'auto';

        // Render user message
        renderMessage('user', message, true);

        // Add to history
        chatHistory.push({
            sender: 'user',
            message: message,
            timestamp: new Date().toISOString()
        });

        // Show typing indicator
        showTypingIndicator();
        isTyping = true;
        sendBtn.disabled = true;

        try {
            // Get user context
            const userContext = getUserContext();

            // Send to API
            const response = await fetch(apiEndpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Current-Page': document.title
                },
                body: JSON.stringify({
                    userMessage: message,
                    userContext: userContext,
                    chatHistory: chatHistory.slice(-10)
                })
            });

            const data = await response.json();

            // Hide typing indicator
            hideTypingIndicator();
            isTyping = false;
            sendBtn.disabled = false;

            if (data.success) {
                // Render assistant message
                renderMessage('assistant', data.responseText, true);

                // Add to history
                chatHistory.push({
                    sender: 'assistant',
                    message: data.responseText,
                    timestamp: new Date().toISOString()
                });

                // Save to localStorage
                saveChatHistory();
            } else {
                renderMessage('assistant', 'عذراً، حدث خطأ: ' + (data.errorMessage || 'غير معروف'), true);
            }
        } catch (e) {
            console.error('Error sending message:', e);
            hideTypingIndicator();
            isTyping = false;
            sendBtn.disabled = false;
            renderMessage('assistant', 'عذراً، حدث خطأ في الاتصال بالخادم. يرجى المحاولة مرة أخرى.', true);
        }
    }

    // Get user context from page
    function getUserContext() {
        // Try to get user info from various sources
        const userName = document.querySelector('[data-user-name]')?.getAttribute('data-user-name') ||
                        document.querySelector('.user-name')?.textContent ||
                        '';

        const userId = document.querySelector('[data-user-id]')?.getAttribute('data-user-id') ||
                      '';

        const role = document.querySelector('[data-user-role]')?.getAttribute('data-user-role') ||
                    document.querySelector('.user-role')?.textContent ||
                    '';

        const tenantId = document.querySelector('[data-tenant-id]')?.getAttribute('data-tenant-id') ||
                        null;

        return {
            userId: userId,
            userName: userName,
            role: role,
            roleName: role,
            currentpageTitle: document.title,
            tenantId: tenantId ? parseInt(tenantId) : null
        };
    }

    // Render message
    function renderMessage(sender, message, animate = true) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `masar-ai-message masar-ai-message-${sender}`;

        const avatarIcon = sender === 'assistant' ? 'bi-robot' : 'bi-person';

        messageDiv.innerHTML = `
            <div class="masar-ai-message-content">
                <div class="masar-ai-message-avatar">
                    <i class="bi ${avatarIcon}"></i>
                </div>
                <div class="masar-ai-message-text">
                    ${formatMessage(message)}
                </div>
            </div>
        `;

        if (animate) {
            messageDiv.style.animation = 'masar-ai-slideIn 0.3s ease';
        }

        messagesContainer.appendChild(messageDiv);
        scrollToBottom();
    }

    // Format message (handle basic markdown)
    function formatMessage(message) {
        // Convert line breaks
        let formatted = message.replace(/\n/g, '<br>');

        // Bold text
        formatted = formatted.replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>');

        // Lists
        formatted = formatted.replace(/^-\s+(.*$)/gm, '<li>$1</li>');
        formatted = formatted.replace(/(<li>.*<\/li>)/s, '<ul>$1</ul>');

        return formatted;
    }

    // Show typing indicator
    function showTypingIndicator() {
        const typingDiv = document.createElement('div');
        typingDiv.className = 'masar-ai-message masar-ai-message-assistant';
        typingDiv.id = 'masar-ai-typing';

        typingDiv.innerHTML = `
            <div class="masar-ai-message-content">
                <div class="masar-ai-message-avatar">
                    <i class="bi bi-robot"></i>
                </div>
                <div class="masar-ai-typing">
                    <div class="masar-ai-typing-dot"></div>
                    <div class="masar-ai-typing-dot"></div>
                    <div class="masar-ai-typing-dot"></div>
                </div>
            </div>
        `;

        messagesContainer.appendChild(typingDiv);
        scrollToBottom();
    }

    // Hide typing indicator
    function hideTypingIndicator() {
        const typingIndicator = document.getElementById('masar-ai-typing');
        if (typingIndicator) {
            typingIndicator.remove();
        }
    }

    // Scroll to bottom
    function scrollToBottom() {
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();
