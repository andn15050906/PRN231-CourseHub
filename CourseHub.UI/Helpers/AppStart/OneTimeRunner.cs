using CourseHub.UI.Services.Contracts.CourseServices;

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

    public static async Task PrepareFirstUse(IHost host)
    {
        //...
        //new HttpClient().GetAsync("https://localhost:7277/api/courses/Resource/Media/CourseMedia/69746c85-6109-4370-9334-1490cd2334b0/ce7c409d-a615-4336-b430-8c5bd17d927c.mp4");

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        var categoryService = services.GetRequiredService<ICategoryApiService>();
        await categoryService.ForgeGet(Global.Categories);
	}
}
