using CourseHub.Core.Entities.SocialDomain.Enums;

namespace CourseHub.Core.Models.Social;

public class ChatMessageModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }

    public string Content { get; set; }
    public MessageStatus Status { get; set; }
    
    public Guid ConversationId { get; set; }
    public List<Reaction> Reactions { get; set; }
}
