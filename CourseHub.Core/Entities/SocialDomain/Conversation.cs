namespace CourseHub.Core.Entities.SocialDomain;

public class Conversation : CreationAuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public bool IsPrivate { get; set; }
    public string AvatarUrl { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public ICollection<ConversationMember> Members { get; set; }
    public ICollection<ChatMessage> ChatMessages { get; set; }
}
