using Microsoft.AspNetCore.Http.Features;

namespace CourseHub.API.Helpers.Http;

public static class HttpContextExtensions
{
    public static string? GetClientIPAddress(this HttpContext context)
    {
        if (!string.IsNullOrWhiteSpace(context.Request.Headers["X-Forwarded-For"]))
            return context.Request.Headers["X-Forwarded-For"];
        return context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
    }
}
