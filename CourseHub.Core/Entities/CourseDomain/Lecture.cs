namespace CourseHub.Core.Entities.CourseDomain;

#pragma warning disable CS8618

public class Lecture : TimeAuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsPreviewable { get; set; }

    // FKs
    public Guid SectionId { get; set; }

    // Navigations
    public Section? Section { get; set; }
    public List<LectureMaterial> Materials { get; set; }
    public List<Comment> Comments { get; set; }

#pragma warning restore CS8618

    public Lecture()
    {

    }

    public Lecture(Guid sectionId, string title, string content, List<LectureMaterial> materials)
    {
        SectionId = sectionId;
        Title = title;
        Content = content;
        Materials = materials;
    }
}