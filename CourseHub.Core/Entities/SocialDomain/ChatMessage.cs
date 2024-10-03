using CourseHub.Core.Entities.SocialDomain.Enums;

namespace CourseHub.Core.Entities.SocialDomain;

public class ChatMessage : AuditedEntity
{
	// Attributes
	public string Content { get; set; }
	public MessageStatus Status { get; set; }

	// FKs
	public Guid ConversationId { get; set; }

	// Navigations
	public User? Creator { get; set; }
	public Conversation? Conversation { get; set; }
	public List<Reaction> Reactions { get; set; }
	// List<Attachment>



	public ChatMessage()
	{

	}

	public ChatMessage(Guid id, Guid creatorId, string content, Guid conversationId)
    {
		Id = id;
		CreatorId = creatorId;
        Content = content;
        Status = MessageStatus.Delivered;
        ConversationId = conversationId;
    }
}
