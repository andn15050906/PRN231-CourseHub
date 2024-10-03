namespace CourseHub.Core.Services.Storage;

public class UserStorage
{
    public static string GetAvatarPath(Guid imgUrl) => $"Avatars/{imgUrl}.jpg";
}
