using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.RequestDtos.Common.NotificationDtos;

public class CreateNotificationDto
{
    public string? Message { get; set; }
    public NotificationType Type { get; set; }
    public Guid? ReceiverId { get; set; }
}