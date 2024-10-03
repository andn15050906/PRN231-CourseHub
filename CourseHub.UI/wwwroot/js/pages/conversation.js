import { HubConnection, MessagingHandler, MESSAGE_TYPES } from '../apis/hubGateway.js';
import { getClientData } from '../common/storage.js';
import { createMessage } from '../pages/conversationElements.js';
import { ChatHandler, ChatMessage } from '../chat/chatHandler.js';



var _messagingHandler = new MessagingHandler(new HubConnection());
var _chatStateHandler = new ChatHandler(document.getElementById('app-message-lst'), createMessage, []);



var _conversationId = window.conversationId;
var _client = getClientData();
var _chatInp;



setupWindow();
setupMessagingHandler(_messagingHandler);



function setupWindow() {
    _chatInp = document.getElementById('app-chat-input');

    _chatInp.onkeydown = (e) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            send();
        }
    }

    var sendBtn = document.getElementById("app-send-btn");
    sendBtn.onclick = () => {
        send();
    }
}

function send() {
    if (_chatInp.value.length == 0) {
        console.log("Empty message!");
        return;
    }

    // add as client-sent message
    let message = new ChatMessage(_chatInp.value, null);
    message.setCreationData(_client.id);
    //_chatStateHandler.addMessage(message);

    // send message
    _messagingHandler.createChatMessage(_conversationId, _chatInp.value, null);

    _chatInp.value = "";
}



function setupMessagingHandler(messagingHandler) {
    messagingHandler.onChatMessageCreated = (conversationId, messageModel) => {
        if (conversationId == _conversationId) {
            _chatStateHandler.addMessage(messageModel);
        }
    }
}