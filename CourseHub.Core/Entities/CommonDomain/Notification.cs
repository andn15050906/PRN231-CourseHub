using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.Entities.CommonDomain;

public class Notification : CreationAuditedEntity
{
    // Attributes
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }

    // FKs
    public Guid? ReceiverId { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public User? Receiver { get; set; }

    public Notification()
    {

    }

    public Notification(Guid sender, string? message, NotificationType type, Guid? receiver = null)
    {
        CreatorId = sender;
        Message = message is null ? string.Empty : message;
        Type = type;
        Status = NotificationStatus.None;
        if (receiver != null)
            ReceiverId = (Guid)receiver;
    }
}
