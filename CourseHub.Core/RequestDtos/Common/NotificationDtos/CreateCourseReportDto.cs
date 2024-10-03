namespace CourseHub.Core.RequestDtos.Common.NotificationDtos;

public class CreateCourseReportDto
{
    public string Message { get; set; }
    public Guid Course { get; set; }
}
