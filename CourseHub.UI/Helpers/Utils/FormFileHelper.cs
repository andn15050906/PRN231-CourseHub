namespace CourseHub.UI.Helpers.Utils;

public static class FormFileHelper
{
    public static StreamContent AsStreamContent(this IFormFile file)
    {
        StreamContent sContent = new(file.OpenReadStream());
        sContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        return sContent;
    }
}
