using CourseHub.Core.RequestDtos.Assignment.McqQuestionDtos;

namespace CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;

public class UpdateAssignmentDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? Duration { get; set; }
    public List<CreateMcqQuestionDto>? Questions { get; set; }
    public double? GradeToPass { get; set; }
}
