using CourseHub.Core.Entities.CourseDomain.Enums;

namespace CourseHub.Core.Entities.CourseDomain;

public class LectureMaterial : DomainObject
{
    public LectureMaterialType Type { get; set; }
    public string Url { get; set; }
}
