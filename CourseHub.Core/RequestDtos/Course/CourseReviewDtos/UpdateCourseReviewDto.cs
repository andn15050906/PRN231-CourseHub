namespace CourseHub.Core.RequestDtos.Course.CourseReviewDtos;

public class UpdateCourseReviewDto
{
    public Guid Id { get; set; }

    public byte Rating { get; set; }

    public string Content { get; set; }
}
