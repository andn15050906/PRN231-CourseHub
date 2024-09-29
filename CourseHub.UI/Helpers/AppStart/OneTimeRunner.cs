namespace CourseHub.UI.Helpers.AppStart;

public static class OneTimeRunner
{
    private static bool _initedConfig;

    public static void InitConfig(WebApplicationBuilder builder)
    {
        if (_initedConfig)
            return;

        Configurer.Init(builder.Configuration);

        _initedConfig = true;
    }
}
