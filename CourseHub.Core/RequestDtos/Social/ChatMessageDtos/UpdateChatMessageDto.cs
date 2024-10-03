using CourseHub.Core.Entities.SocialDomain.Enums;

namespace CourseHub.Core.RequestDtos.Social.ChatMessageDtos;

public class UpdateChatMessageDto
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public MessageStatus? Status { get; set; }
    public List<Guid>? Attachments { get; set; }
}
