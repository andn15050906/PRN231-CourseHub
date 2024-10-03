using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.Entities.CommonDomain;

#pragma warning disable CS8618

public class Reaction : CreationAuditedDomainObject
{
    // Keys (with CreatorId from base)
    public Guid SourceEntityId { get; set; }

    // Attributes
    public string Content { get; set; }
    public ReactionSourceEntityType SourceType { get; set; }

#pragma warning restore CS8618



    public Reaction()
    {

    }

    public Reaction(Guid creatorId, Guid sourceEntityId, string content, ReactionSourceEntityType sourceType)
    {
        CreatorId = creatorId;
        SourceEntityId = sourceEntityId;
        Content = content;
        SourceType = sourceType;
    }



    public void SetPath(Guid sourceEntityId, ReactionSourceEntityType type)
    {
        SourceEntityId = sourceEntityId;
        SourceType = type;
    }
}
