using CourseHub.Core.Interfaces.Logging;
using System.Web;

namespace CourseHub.Core.Services.Storage;

public static class ServerStorage
{
    private const string BASE_PATH = "wwwroot/";






    internal static async Task WriteText(string filePath, string content)
    {
        await File.WriteAllTextAsync(GetPhysicalPath(filePath), content);
    }

    internal static async Task<string> ReadText(string filePath)
    {
        return await File.ReadAllTextAsync(GetPhysicalPath(filePath));
    }






    internal static async Task<bool> SaveFile(Stream stream, string filePath, IAppLogger logger)
    {
        filePath = GetPhysicalPath(filePath);

        // clarify DirPath and FilePath
        //string fileName = Path.GetFileName(filePath);
        string dirPath = Path.GetDirectoryName(filePath)!;

        try
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using var createdStream = File.Create(filePath);
            await stream.CopyToAsync(createdStream);
            stream.Dispose();
            return true;
        }
        catch (Exception e)
        {
            logger.Warn(e.Message);
            return false;
        }
    }

    public static Stream? ReadAsStream(string filePath)
    {
        filePath = GetPhysicalPath(filePath);
        if (!File.Exists(filePath))
            return null;
        return File.OpenRead(filePath);
    }

    public static (Stream?, string?) ReadAsStreamWithName(string filePath)
    {
        filePath = GetPhysicalPath(filePath);
        if (!File.Exists(filePath))
            return (null, null);

        return (File.OpenRead(filePath), Path.GetExtension(filePath));
    }

    public static (Stream?, string?) ReadWithoutExtension(string filePath)
    {
        filePath = GetPhysicalPath(filePath);
        DirectoryInfo info = new(Path.GetDirectoryName(filePath)!);
        FileInfo[] matchingFiles = info.GetFiles($"{Path.GetFileName(filePath)}.*");

        if (matchingFiles.Length == 0)
            return (null, null);
        return (matchingFiles[0].OpenRead(), matchingFiles[0].Name);
    }






    /// <summary>
    /// If the dirName (ex: using the book's name for the folder) is invalid,
    /// name the folder by its encoded version instead
    /// </summary>
    internal static string GetValidDirName(string dirName)
    {
        if (dirName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
            return dirName;
        return HttpUtility.UrlEncode(dirName).Replace('%', ' ');
    }






    private static string GetPhysicalPath(string filePath) => BASE_PATH + filePath;
}