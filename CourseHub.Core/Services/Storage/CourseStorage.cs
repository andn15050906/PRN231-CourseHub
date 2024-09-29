namespace CourseHub.Core.Services.Storage;

public class CourseStorage
{
    public static string GetValidPath(string courseName, string section, byte mediaIndex, string mediaExtension)
        => $"{ServerStorage.GetValidDirName(courseName)}/{section}/{mediaIndex}{mediaExtension}";

    public static string GetCourseMediaPath(string url) => "CourseMedia/" + url;
}
