namespace CourseHub.Core.Entities.AssignmentDomain;

public class Assignment : Entity
{
    // Attributes
    public string Name { get; set; }
    public int Duration { get; set; }
    public int QuestionCount { get; set; }
    public double GradeToPass { get; set; }

    // FKs
    public Guid SectionId { get; set; }
    public Guid CreatorId { get; set; }

    // Navigations
    public Section? Section { get; set; }
    public User? Creator { get; set; }
    public ICollection<McqQuestion> Questions { get; set; }



    public Assignment()
    {

    }

    public Assignment(Guid id, string name, int duration, double gradeToPass, Guid sectionId, User? creator, List<McqQuestion> questions)
    {
        Id = id;
        Name = name;
        Duration = duration;
        QuestionCount = questions.Count;
        GradeToPass = gradeToPass;
        SectionId = sectionId;
        Creator = creator;
        Questions = questions;
    }
}
