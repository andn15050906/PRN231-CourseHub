namespace CourseHub.Core.Entities.AssignmentDomain;

public class McqChoice : Entity
{
    // Attributes
    public string Content { get; set; }
    public bool IsCorrect { get; set; }

    public McqChoice()
    {

    }

    public McqChoice(string content, bool isCorrect)
    {
        Content = content;
        IsCorrect = isCorrect;
    }
}
