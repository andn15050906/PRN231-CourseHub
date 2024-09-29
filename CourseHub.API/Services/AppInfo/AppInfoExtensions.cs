using CourseHub.API.Helpers.AppStart;

namespace CourseHub.API.Services.AppInfo;

public static class AppInfoExtensions
{
    public static IServiceCollection AddAppInfo(this IServiceCollection services)
    {
        AppInfoOptions config = Configurer.GetAppInfoOptions();

        services.Configure<AppInfoOptions>(options =>
        {
            options.AppName = config.AppName;
            options.MainFrontendApp = config.MainFrontendApp;
        });

        return services;
    }
}
