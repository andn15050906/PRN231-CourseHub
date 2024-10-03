namespace CourseHub.Core.Models.Assignment.McqQuestionModels;

public class McqQuestionModel
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public List<McqChoice> Choices { get; set; }
}
