using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.Models.Common.NotificationModels;

public class NotificationModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid CreatorId { get; set; }

    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }

    public Guid ReceiverId { get; set; }
}
