using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.RequestDtos.Common.NotificationDtos;

public class UpdateNotificationDto
{
    public Guid Id { get; set; }
    public NotificationStatus Status { get; set; }
}
