using Microsoft.AspNetCore.Http;

namespace CourseHub.Core.RequestDtos.Shared;

public class CreateMediaDto
{
    public string? Url { get; set; }
    public IFormFile? File { get; set; }
}
