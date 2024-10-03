namespace CourseHub.Core.Entities.Contracts;

public abstract class CreationAuditedEntity : Entity
{
    public DateTime CreationTime { get; protected set; }
    public Guid CreatorId { get; protected set; }
}
