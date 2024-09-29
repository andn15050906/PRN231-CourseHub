namespace CourseHub.Core.Entities.AssignmentDomain;

public class McqUserAnswer : DomainObject
{
    // Keys
    public Guid SubmissionId { get; set; }
    public Guid MCQQuestionId { get; set; }

    // Fks
    public Guid MCQChoiceId { get; set; }

    public Submission? Submission { get; set; }
    public McqChoice? MCQChoice { get; set; }
}
