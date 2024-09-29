using CourseHub.API.Helpers.AppStart;

namespace CourseHub.API.Services.Realtime;

public static class RealtimeExtensions
{
    public static IServiceCollection AddRealtimeService(this IServiceCollection services)
    {
        var serverBuilder = services.AddSignalR(_ => _.MaximumReceiveMessageSize = 128000);

        var options = Configurer.GetRealtimeOptions();
        if (options.Enabled != "true")
            return services;

        serverBuilder.AddAzureSignalR(_ => {
            _.ClaimsProvider = context => context.User.Claims;
        });
        return services;
    }
}
