using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.Models.Course.CourseModels;

namespace CourseHub.Core.Models.Course.EnrollmentModels;

public class EnrollmentModel
{
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public CourseStatus Status { get; set; }

    public Guid? BillId { get; set; }

    public CourseOverviewModel? Course { get; set; }
}
