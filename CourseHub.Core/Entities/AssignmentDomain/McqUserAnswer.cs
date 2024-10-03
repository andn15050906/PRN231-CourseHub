namespace CourseHub.Core.Entities.AssignmentDomain;

public class McqUserAnswer : DomainObject
{
    // Keys
    public Guid SubmissionId { get; set; }
    public Guid MCQChoiceId { get; set; }

    // Navigations
    public Submission? Submission { get; set; }
    public McqChoice? MCQChoice { get; set; }
}
