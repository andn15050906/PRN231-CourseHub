namespace CourseHub.Core.Entities.CommonDomain;

public class Notification : CreationAuditedEntity
{
    // Attributes
    public string Message { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }

    // FKs
    public Guid ReceiverId { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public User? Receiver { get; set; }
}
