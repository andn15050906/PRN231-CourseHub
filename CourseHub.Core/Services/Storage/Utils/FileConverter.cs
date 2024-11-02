using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace CourseHub.Core.Services.Storage.Utils;

internal class FileConverter
{
    // using SixLabors.ImageSharp
    // System.Drawing.Common is only supported on Windows

    public const string EXTENSION_JPG = ".jpg";



    public async Task<Stream> ToJpg(IFormFile file)
    {
        if (file.ContentType == "image/jpeg")
            return file.OpenReadStream();

        using var imgStream = new MemoryStream();
        await file.CopyToAsync(imgStream);
        imgStream.Seek(0, SeekOrigin.Begin);
        using var image = await Image.LoadAsync(imgStream);

        // not closing the stream (let it be closed by ServerStorage)
        var jpgStream = new MemoryStream();
        await image.SaveAsJpegAsync(jpgStream);
        jpgStream.Position = 0;
        return jpgStream;
    }
}
