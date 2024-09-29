using CourseHub.UI.Services;

namespace CourseHub.UI.Helpers.AppStart;

public class Configurer
{
    private static ConfigurationManager? _configuration;

    private Configurer() { }

    public static void Init(ConfigurationManager configuration)
    {
        _configuration ??= configuration;
    }

    public static ApiClientOptions GetApiClientOptions()
        => _configuration!.GetSection("ServicePaths").Get<ApiClientOptions>();
}
