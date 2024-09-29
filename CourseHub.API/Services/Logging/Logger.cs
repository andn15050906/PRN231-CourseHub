using CourseHub.Core.Interfaces.Logging;
using Serilog;

namespace CourseHub.API.Services.Logging;

public class Logger : IAppLogger
{
    public void Inform(string message)
    {
        Log.Information(IAppLogger.INFORMATION_TEMPLATE, DateTime.UtcNow, message);
    }

    public void Warn(string message)
    {
        Log.Warning(IAppLogger.INFORMATION_TEMPLATE, DateTime.UtcNow, message);
    }
}
