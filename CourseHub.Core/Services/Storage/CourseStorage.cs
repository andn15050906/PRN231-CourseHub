namespace CourseHub.Core.Services.Storage;

public class CourseStorage
{
    public static string GetValidPath(string courseName, string section, byte mediaIndex, string mediaExtension)
        => $"{ServerStorage.GetValidDirName(courseName)}/{section}/{mediaIndex}{mediaExtension}";



    public static string GetCourseThumbPath(Guid courseId)
        => $"CourseThumb/{courseId}.jpg";



    public static string GetCourseMediaPath(Guid courseId, Guid fileName, string extension)
        => $"CourseMedia/{courseId}/{fileName}{extension}";

    public static string GetCourseMediaPathWithoutExtension(Guid courseId, Guid fileName)
        => $"CourseMedia/{courseId}/{fileName}";
}
