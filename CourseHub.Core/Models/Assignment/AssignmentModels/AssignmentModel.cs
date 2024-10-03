using CourseHub.Core.Models.Assignment.McqQuestionModels;
using CourseHub.Core.Models.Course.SectionModels;

namespace CourseHub.Core.Models.Assignment.AssignmentModels;

public class AssignmentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public int QuestionCount { get; set; }
    public double GradeToPass { get; set; }

    public SectionMinModel? Section { get; set; }
    public List<McqQuestionModel> Questions { get; set; }
}
