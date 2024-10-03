namespace CourseHub.Core.RequestDtos.Course.CourseReviewDtos;

public class QueryCourseReviewDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;

    public Guid? CourseId { get; set; }
    public Guid? CreatorId { get; set; }
}
