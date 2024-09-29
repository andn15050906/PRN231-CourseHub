namespace CourseHub.UI.Helpers.Utils;

public static class ResourceHelper
{
    public static bool IsRemote(string resourceUrl)
    {
        if (string.IsNullOrEmpty(resourceUrl))
            return false;
        return resourceUrl.StartsWith("http");
    }

    public static bool IsVideo(IFormFile file)
    {
        var allowedVideoExtensions = new string[] { ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedVideoExtensions.Contains(fileExtension))
            return false;

        var allowedVideoContentTypes = new string[] { "video/mp4", "video/avi", "video/quicktime", "video/x-matroska", "video/x-ms-wmv", "video/x-flv", "video/webm" };
        if (!allowedVideoContentTypes.Contains(file.ContentType.ToLowerInvariant()))
            return false;

        return true;
    }

    public static bool IsImage(IFormFile file)
    {
        var allowedImageExtensions = new string[] { ".jpg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedImageExtensions.Contains(fileExtension))
            return false;

        var allowedImageContentTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
        if (!allowedImageContentTypes.Contains(file.ContentType.ToLowerInvariant()))
            return false;

        return true;
    }
}
