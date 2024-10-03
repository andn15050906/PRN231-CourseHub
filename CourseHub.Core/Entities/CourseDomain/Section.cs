namespace CourseHub.Core.Entities.CourseDomain;

#pragma warning disable CS8618

public class Section : TimeAuditedEntity
{
    // Attributes
    public byte Index { get; set; }
    public string Title { get; set; }
    public byte LectureCount { get; private set; }

    // FKs
    public Guid CourseId { get; set; }

    // Navigations
    public Course Course { get; set; }
    public ICollection<Lecture> Lectures { get; private set; }
    //public Assignment? Assignment { get; private set; }

    public Section()
    {

    }

    public Section(byte index, string title)
    {
        Index = index;
        Title = title;
    }
}

#pragma warning restore CS8618