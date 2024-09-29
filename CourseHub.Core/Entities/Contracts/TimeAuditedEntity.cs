namespace CourseHub.Core.Entities.Contracts;

public abstract class TimeAuditedEntity : Entity
{
    public DateTime CreationTime { get; protected set; }
    public DateTime LastModificationTime { get; protected set; }
}
