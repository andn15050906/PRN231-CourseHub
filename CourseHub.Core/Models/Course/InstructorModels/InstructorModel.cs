namespace CourseHub.Core.Models.Course.InstructorModels;

public class InstructorModel
{
    public Guid Id { get; set; }
    public string Intro { get; set; }
    public string Experience { get; set; }
    public long Balance { get; set; }
    public byte CourseCount { get; set; }
    public Guid CreatorId { get; set; }
}
