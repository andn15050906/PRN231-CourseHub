using CourseHub.Core.Entities.CommonDomain.Enums;

namespace CourseHub.Core.Entities.CommonDomain;

public class CommentMedia : DomainObject
{
    public CommentMediaType Type { get; set; }
    public string Url { get; set; }
}
