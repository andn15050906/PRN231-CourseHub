using CourseHub.Core.Entities.CourseDomain.Enums;

namespace CourseHub.Core.Entities.CourseDomain;

public class Enrollment : CreationAuditedDomainObject
{
    // Keys (with CreatorId from base)
    public Guid CourseId { get; set; }

    // Attributes
    public CourseStatus Status { get; set; }

    // FKs
    public Guid? BillId { get; set; }

    // Navigation
    public Course? Course { get; set; }
    public Bill? Bill { get; set; }
}
