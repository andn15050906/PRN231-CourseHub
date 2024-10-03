using CourseHub.Core.Models.Course.LectureModels;

namespace CourseHub.Core.Models.Course.SectionModels;

public class SectionModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }

    public byte Index { get; set; }
    public string Title { get; set; }
    public byte LectureCount { get; private set; }

    public List<LectureModel> Lectures { get; set; }
}
