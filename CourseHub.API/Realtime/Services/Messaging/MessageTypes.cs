namespace CourseHub.API.Realtime.Services.Messaging;

public enum MessageTypes : byte
{
    // Send
    CreateConversation,
    UpdateConversation,

    CreateTextChatMessage,
    CreateFileChatMessage,



    // Receive
    ChatMessageCreated
}