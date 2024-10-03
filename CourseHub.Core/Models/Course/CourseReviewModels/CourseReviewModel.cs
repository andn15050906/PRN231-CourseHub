namespace CourseHub.Core.Models.Course.CourseReviewModels;

public class CourseReviewModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }
    public Guid CreatorId { get; set; }

    public string Content { get; set; }
    public byte Rating { get; set; }
    public Guid CourseId { get; set; }
}
