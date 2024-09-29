namespace CourseHub.Core.RequestDtos.Course.CourseDtos;

public class QueryCourseDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;
}
