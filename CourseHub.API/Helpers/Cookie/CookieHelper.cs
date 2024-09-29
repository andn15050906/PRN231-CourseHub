namespace CourseHub.API.Helpers.Cookie;

public class CookieHelper
{
    private static CookieHelperOptions? _options;

    private CookieHelper() { }

    public static void Init(CookieHelperOptions? options)
    {
        _options ??= options;
    }

    public static CookieOptions GetOptions() => new()
    {
        SameSite = SameSiteMode.None,
        Secure = _options!.Secure,
        Expires = DateTime.UtcNow.AddMinutes(_options.Expires)
    };

    public static CookieOptions GetExpiredOptions() => new()
    {
        SameSite = SameSiteMode.None,
        Secure = _options!.Secure,
        Expires = DateTime.UtcNow
    };
}
