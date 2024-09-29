namespace CourseHub.Core.Entities.CourseDomain;

#pragma warning disable CS8618

public class Section : TimeAuditedEntity
{
    // Attributes
    public byte Index { get; set; }
    public string Title { get; set; }
    public byte LectureCount { get; private set; }

    // Navigations
    public ICollection<Lecture> Lectures { get; private set; }
    //public Assignment? Assignment { get; private set; }
}

#pragma warning restore CS8618