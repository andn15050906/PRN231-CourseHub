namespace CourseHub.Core.Entities.CourseDomain;

#pragma warning disable CS8618

public class Lecture : TimeAuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Content { get; set; }

    // Navigations
    public ICollection<LectureMaterial> Materials { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

#pragma warning restore CS8618