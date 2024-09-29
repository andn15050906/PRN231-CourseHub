using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.RequestDtos.Common.NotificationDtos;

public class QueryNotificationDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;

    public Guid? CreatorId { get; set; }
    public Guid? ReceiverId { get; set; }
    public NotificationType? Type { get; set; }
    public NotificationStatus? Status { get; set; }
}
