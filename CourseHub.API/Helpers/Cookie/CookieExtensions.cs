using System.Security.Claims;

namespace CourseHub.API.Helpers.Cookie;

public static class CookieExtensions
{
    public const string BEARER = "Bearer";
    public const string REFRESH = "Refresh";

    /// <summary>
    /// Extension method to get NameIdentifier
    /// /// </summary>
    public static Guid? GetClientId(this HttpContext httpContext)
    {
        foreach (Claim claim in httpContext.User.Claims)
            if (claim.Type == ClaimTypes.NameIdentifier)
                // there might be multiple NameIdentifiers
                if (Guid.TryParse(claim.Value, out Guid result))
                    return result;
        return null;
    }

    /// <summary>
    /// Extension method to set AccessToken and RefreshToken
    /// </summary>
    public static void SetCredentials(this HttpResponse httpResponse, string accessToken, string refreshToken, CookieOptions options)
    {
        options.HttpOnly = true;
        httpResponse.Cookies.Append(BEARER, accessToken, options);
        httpResponse.Cookies.Append(REFRESH, refreshToken, options);
    }

    /// <summary>
    /// Extension method to get AccessToken
    /// </summary>
    public static string? GetAccessToken(this HttpRequest httpRequest)
        => httpRequest.Cookies[BEARER];

    /// <summary>
    /// Extension method to get RefreshToken
    /// </summary>
    public static string? GetRefreshToken(this HttpRequest httpRequest)
        => httpRequest.Cookies[REFRESH];
}