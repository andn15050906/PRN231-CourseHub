using CourseHub.Core.Entities.CourseDomain.Enums;

namespace CourseHub.Core.Entities.CourseDomain;

public class CourseMeta : DomainObject
{
    public CourseMetaType Type { get; set; }
    public string Value { get; set; }
}
