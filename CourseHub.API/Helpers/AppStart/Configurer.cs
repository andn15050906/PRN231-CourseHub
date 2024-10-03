using CourseHub.API.Helpers.Cookie;
using CourseHub.API.Services.AppInfo;
using CourseHub.API.Services.Authentication;
using CourseHub.API.Services.Email;
using CourseHub.API.Services.External.Payment;
using CourseHub.API.Services.Realtime;
using CourseHub.Core.Interfaces.Authentication;

namespace CourseHub.API.Helpers.AppStart;

public class Configurer
{
    private static ConfigurationManager? _configuration;

    private Configurer() { }

    public static void Init(ConfigurationManager configuration)
    {
        _configuration ??= configuration;
    }

    public static string GetContextConnectionString()
    {
        string targetConnectionString = _configuration.GetValue<string>("TargetConnectionStrings:Context");
        return _configuration!.GetConnectionString(targetConnectionString)!;
    }

    /*public static string GetHangfireConnectionString()
    {
        string targetConnectionString = _configuration.GetValue<string>("TargetConnectionStrings:Hangfire");
        return _configuration!.GetConnectionString(targetConnectionString)!;
    }*/

    public static CookieHelperOptions GetCookieConfigOptions()
        => _configuration!.GetSection("CookieOptions").Get<CookieHelperOptions>();

    public static TokenOptions GetTokenOptions()
        => _configuration!.GetSection("JwtOptions").Get<TokenOptions>();

    public static EmailOptions GetEmailOptions()
        => _configuration!.GetSection("External:Gmail").Get<EmailOptions>();

    public static OAuthOptions GetGoogleOAuthOptions()
        => _configuration!.GetSection("External:OAuth:Google").Get<OAuthOptions>();

    public static PaymentOptions GetPaymentOptions()
        => _configuration!.GetSection("External:Payment:VNPay").Get<PaymentOptions>();

    public static AppInfoOptions GetAppInfoOptions()
        => _configuration!.GetSection("AppInfo").Get<AppInfoOptions>();

    public static string[] GetCorsOrigins()
        => _configuration!.GetSection("CORS").Get<string[]>();

    public static RealtimeOptions GetRealtimeOptions()
        => _configuration!.GetSection("Azure:SignalR").Get<RealtimeOptions>();
}
