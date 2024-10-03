namespace CourseHub.Core.Entities.Contracts;

public abstract class AuditedEntity : TimeAuditedEntity
{
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
}
