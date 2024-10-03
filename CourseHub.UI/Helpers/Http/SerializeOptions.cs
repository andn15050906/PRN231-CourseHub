using System.Text.Json;

namespace CourseHub.UI.Helpers.Http;

public static class SerializeOptions
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static readonly JsonSerializerOptions JsonCamelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
