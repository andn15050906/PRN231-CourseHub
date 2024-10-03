using CourseHub.Core.Entities.CourseDomain.Enums;

namespace CourseHub.Core.RequestDtos.Course.CourseDtos;

public class QueryCourseDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;

    public string? Title { get; set; }
    public CourseStatus? Status { get; set; }
    public CourseLevel? Level { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? InstructorId { get; set; }

    // Order By
    public bool ByPrice { get; set; }
    public bool ByDiscount { get; set; }
    public bool ByLearnerCount { get; set; }
    public bool ByAvgRating { get; set; }
}
