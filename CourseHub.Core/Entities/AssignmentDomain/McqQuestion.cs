namespace CourseHub.Core.Entities.AssignmentDomain;

/// <summary>
/// Mc"q"Question instead of McQuestion for better clarity
/// </summary>
public class McqQuestion : Entity
{
    // Attributes
    public string Content { get; set; }

    // FKs
    public Guid AssignmentId { get; set; }

    // Navigations
    public ICollection<McqChoice> Choices { get; set; }

    public McqQuestion()
    {

    }

    public McqQuestion(string content, Guid assignmentId, List<McqChoice> choices)
    {
        Content = content;
        AssignmentId = assignmentId;
        Choices = choices;
    }
}
