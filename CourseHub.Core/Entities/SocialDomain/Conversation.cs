namespace CourseHub.Core.Entities.SocialDomain;

public class Conversation : CreationAuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public bool IsPrivate { get; set; }
    public string AvatarUrl { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public List<ConversationMember> Members { get; set; }
    public List<ChatMessage> ChatMessages { get; set; }



    public Conversation()
    {

    }

    public Conversation(Guid id, Guid creatorId, string title, bool isPrivate, string avatarUrl, List<ConversationMember> members)
    {
        Id = id;
        CreatorId = creatorId;
        Title = title;
        IsPrivate = isPrivate;
        AvatarUrl = avatarUrl;
        Members = members;
    }
}
