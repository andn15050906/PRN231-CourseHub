namespace CourseHub.Core.Services.Storage;

public class CommentStorage
{
    public static string GetMediaPath(Guid commentId, Guid fileName, string extension)
        => $"CommentMedia/{commentId}/{fileName}{extension}";
}
