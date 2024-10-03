namespace CourseHub.Core.Entities.Contracts;

public abstract class CreationAuditedDomainObject : DomainObject
{
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
}
