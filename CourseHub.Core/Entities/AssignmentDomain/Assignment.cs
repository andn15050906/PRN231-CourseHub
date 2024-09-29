namespace CourseHub.Core.Entities.AssignmentDomain;

public class Assignment : Entity
{
    // Attributes
    public string Name { get; set; }
    public int Duration { get; set; }
    public int QuestionCount { get; set; }

    // FKs
    public Guid SectionId { get; set; }

    // Navigations
    public Section? Section { get; set; }
    public User? Creator { get; set; }
    public ICollection<McqQuestion> Questions { get; set; }
}
