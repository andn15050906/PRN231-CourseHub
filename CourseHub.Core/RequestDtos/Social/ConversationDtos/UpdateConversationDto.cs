using CourseHub.Core.RequestDtos.Shared;

namespace CourseHub.Core.RequestDtos.Social.ConversationDtos;

public class UpdateConversationDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public CreateMediaDto Avatar { get; set; }

    public Guid? AddedAdmin { get; set; }
    public Guid? RemovedAdmin { get; set; }
    public List<Guid>? AddedParticipants { get; set; }
    public List<Guid>? RemovedParticipants { get; set; }
}
