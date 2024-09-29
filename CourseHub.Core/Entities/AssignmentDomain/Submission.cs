namespace CourseHub.Core.Entities.AssignmentDomain;

public class Submission : AuditedEntity
{
    // Attributes
    public double Mark { get; set; }
    public int TimeSpentInSec { get; set; }

    // FKs
    public Guid AssignmentId { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public Assignment? Assignment { get; set; }
    public ICollection<McqUserAnswer> Answers { get; set; }
}
