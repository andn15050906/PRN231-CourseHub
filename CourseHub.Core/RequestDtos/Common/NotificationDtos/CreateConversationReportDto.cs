namespace CourseHub.Core.RequestDtos.Common.NotificationDtos;

public class CreateConversationReportDto
{
    public string Message { get; set; }
    public Guid Conversation { get; set; }
}
