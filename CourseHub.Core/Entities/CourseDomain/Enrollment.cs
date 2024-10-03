using CourseHub.Core.Entities.CourseDomain.Enums;

namespace CourseHub.Core.Entities.CourseDomain;

public class Enrollment : CreationAuditedDomainObject
{
    // Keys (with CreatorId from base)
    public Guid CourseId { get; set; }

    // Attributes
    public CourseStatus Status { get; set; }
    public string LectureMilestones { get; set; }
    public string AssignmentMilestones { get; set; }
    public string SectionMilestones { get; set; }

    // FKs
    public Guid? BillId { get; set; }

    // Navigation
    public User? Creator { get; set; }
    public Course? Course { get; set; }
    public Bill? Bill { get; set; }



    public Enrollment()
    {

    }

    public Enrollment(Guid courseId, Guid creator, Guid billId)
    {
        CourseId = courseId;
        CreatorId = creator;
        BillId = billId;

        LectureMilestones = string.Empty;
        AssignmentMilestones = string.Empty;
        SectionMilestones = string.Empty;
    }
}
