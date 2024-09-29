namespace CourseHub.Core.Entities.Contracts;

public abstract class AuditedEntity : TimeAuditedEntity
{
    public Guid CreatorId { get; protected set; }
    public Guid LastModifierId { get; protected set; }
}
