namespace CourseHub.Core.RequestDtos.Common.CommentDtos;

public class QueryCommentDto
{
    public short PageIndex { get; set; }    // from 0
    public byte PageSize { get; set; } = 20;

    public Guid? LectureId { get; set; }
    public Guid? CreatorId { get; set; }
}
