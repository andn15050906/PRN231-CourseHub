using CourseHub.Core.Entities.CommonDomain.Enums;
using CourseHub.Core.RequestDtos.Shared;

namespace CourseHub.Core.RequestDtos.Common.CommentDtos;

public class CreateCommentDto
{
    public string Content { get; set; }
    public CommentSourceEntityType SourceType { get; set; }
    public Guid SourceId { get; set; }
    public List<CreateCommentMediaDto>? Medias { get; set; }

    public class CreateCommentMediaDto
    {
        public CreateMediaDto Dto { get; set; }
        public CommentMediaType Type { get; set; }
    }
}
