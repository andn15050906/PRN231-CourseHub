using CourseHub.Core.Models.Common.CommentModels;

namespace CourseHub.Core.Models.Course.LectureModels;

public class LectureModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; protected set; }
    public DateTime LastModificationTime { get; protected set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsPreviewable { get; set; }
    public List<LectureMaterial> Materials { get; set; }
    public List<CommentModel> Comments { get; set; }
}
