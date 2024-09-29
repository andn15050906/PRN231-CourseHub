namespace CourseHub.UI.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseAdminViewMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AdminViewMiddleware>();
    }
}