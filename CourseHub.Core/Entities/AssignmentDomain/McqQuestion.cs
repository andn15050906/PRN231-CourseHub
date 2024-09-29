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
}
