using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.RequestDtos.Common.NotificationDtos;

public class CreateMultipleNotificationDto
{
    public NotificationType Type { get; set; }

    public string? Message { get; set; }
    public List<Guid> ReceiverIds { get; set; }
}
