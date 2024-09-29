namespace CourseHub.Core.Entities.Contracts;

public abstract class CreationAuditedDomainObject : DomainObject
{
    public Guid CreatorId { get; protected set; }
    public DateTime CreationTime { get; protected set; }
}
