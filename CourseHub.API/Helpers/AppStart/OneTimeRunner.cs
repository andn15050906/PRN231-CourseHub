using CourseHub.API.Helpers.Cookie;

namespace CourseHub.API.Helpers.AppStart;

public static class OneTimeRunner
{
    private static bool _initedConfig;

    public static void InitConfig(WebApplicationBuilder builder)
    {
        if (_initedConfig)
            return;

        Services.Logging.LoggerExtensions.ConfigLogger(builder);
        Configurer.Init(builder.Configuration);
        CookieHelper.Init(Configurer.GetCookieConfigOptions());

        _initedConfig = true;
    }
}
