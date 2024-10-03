namespace CourseHub.Core.Models.Assignment.SubmissionModels;

public class SubmissionModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }

    public double Mark { get; set; }
    public int TimeSpentInSec { get; set; }

    public Guid AssignmentId { get; set; }

    public List<McqUserAnswer> Answers { get; set; }
}
