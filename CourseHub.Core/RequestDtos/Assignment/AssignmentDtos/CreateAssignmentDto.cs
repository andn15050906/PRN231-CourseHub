using CourseHub.Core.RequestDtos.Assignment.McqQuestionDtos;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;

public class CreateAssignmentDto
{
    public Guid SectionId { get; set; }

    [Required]
    public string Name { get; set; }

    public int Duration { get; set; } = 60;

    public List<CreateMcqQuestionDto> Questions { get; set; }

    public double GradeToPass { get; set; } = 8;
}
