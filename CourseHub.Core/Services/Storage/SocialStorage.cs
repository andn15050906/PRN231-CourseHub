namespace CourseHub.Core.Services.Storage;

public class SocialStorage
{
    public static string GetAvatarPath(Guid conversationId)
        => $"Conversations/{conversationId}.jpg";
}