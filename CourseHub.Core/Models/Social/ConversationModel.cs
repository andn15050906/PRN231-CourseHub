namespace CourseHub.Core.Models.Social;

public class ConversationModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid CreatorId { get; set; }

    public string Title { get; set; }
    public bool IsPrivate { get; set; }
    public string AvatarUrl { get; set; }

    public List<ConversationMember> Members { get; set; }
}
