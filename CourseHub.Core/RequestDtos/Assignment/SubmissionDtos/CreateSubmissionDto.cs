namespace CourseHub.Core.RequestDtos.Assignment.SubmissionDtos;

public class CreateSubmissionDto
{
    public class CreateMcqUserAnswerDto
    {
        public Guid MCQChoiceId { get; set; }
    }

    public int TimeSpentInSec { get; set; }
    public Guid AssignmentId { get; set; }
    public List<CreateMcqUserAnswerDto> Answers { get; set; }
}
