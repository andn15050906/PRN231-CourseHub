using Serilog;

namespace CourseHub.API.Services.Logging;

/// <summary>
/// Use Core.Interfaces.Logging.ILogger instead of Microsoft.Extensions.Logging.ILogger
/// to avoid adding generic type parameter
/// </summary>
public static class LoggerExtensions
{
    public static void ConfigLogger(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(
                "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileTimeLimit: TimeSpan.FromDays(7),
                fileSizeLimitBytes: 536870912)                  // 512 MB
            .CreateLogger();

        Log.Information("__Starting web host");

        builder.Host.UseSerilog();
    }

    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        services.AddSingleton<Core.Interfaces.Logging.IAppLogger, Logger>();
        return services;
    }
}
