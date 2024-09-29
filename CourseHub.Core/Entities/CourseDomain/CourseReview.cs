namespace CourseHub.Core.Entities.CourseDomain;

public class CourseReview : AuditedEntity
{
    // Attributes
    public string Content { get; set; }
    public byte Rating { get; set; }

    // FKs
    public Guid CourseId { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public Course? Course { get; set; }
}
