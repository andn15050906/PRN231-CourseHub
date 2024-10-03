using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;

namespace CourseHub.UI.Middlewares;

public class AdminViewMiddleware
{

    private readonly RequestDelegate _next;

    public AdminViewMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.ToString().ToLower().Contains("/admin"))
        {
            var clientData = await context.GetClientData();
            if (clientData is null)
            {
                context.Response.Redirect(Global.PAGE_SIGNIN);
                return;
            }
            if (clientData.Role < Core.Entities.UserDomain.Enums.Role.Admin)
            {
                context.Response.StatusCode = 403;
                return;
            }
        }

        await _next(context);
    }
}