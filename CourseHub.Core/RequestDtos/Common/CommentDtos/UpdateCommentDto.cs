namespace CourseHub.Core.RequestDtos.Common.CommentDtos;

public class UpdateCommentDto
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    /*
    public List<CreateCommentMediaDto>? Medias { get; set; }

    public class CreateCommentMediaDto
    {
        public CreateMediaDto Dto { get; set; }
        public CommentMediaType Type { get; set; }
    }*/
}
