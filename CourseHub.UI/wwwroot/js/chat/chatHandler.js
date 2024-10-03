export { ChatMessage, ChatHandler }

// not ChatMessageDTO
class ChatMessage {
    //id;
    //senderId;
    creatorId;
    content;
    createdAt;
    //updatedAt;
    //status;
    attachments;
    //reactions;

    constructor(content, attachments) {
        this.content = content;
        this.attachments = attachments;
    }

    setCreationData(creatorId) {
        this.creatorId = creatorId;
        this.createdAt = new Date().toLocaleString();;
    }
}

class ChatHandler {
    #messagesContainer;
    #messages;

    #createMessageCallback;

    constructor(messagesContainer, createMessageCallback, messages) {
        this.#messagesContainer = messagesContainer;
        this.#createMessageCallback = createMessageCallback;
        this.#messages = messages;
    }

    addMessage(message) {
        this.#messages.push(message);
        this.#messagesContainer.appendChild(this.#createMessageCallback(message));
    }
}