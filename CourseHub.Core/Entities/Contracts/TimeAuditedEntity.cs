namespace CourseHub.Core.Entities.Contracts;

public abstract class TimeAuditedEntity : Entity
{
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
}
