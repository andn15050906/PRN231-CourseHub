using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.Services.Domain.UserServices.TempModels;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CourseHub.UI.Helpers.Utils;

public static class IdentityExtensions
{
    private const string BEARER_COOKIE_NAME = "Bearer";
    private const string REFRESH_COOKIE_NAME = "Refresh";
    private const string REMEMBER_COOKIE_NAME = "Remember";
    private const string SESSION_USERINFO = "userInfo";

    private static readonly CookieOptions credentialOptions = new()
    {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(1),
        Path = "/",
        Secure = true,
        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
    };

    private static readonly CookieOptions expiredOptions = new()
    {
        HttpOnly = true,
        Expires = DateTime.UtcNow,
        Secure = false,
        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
    };

    private static readonly CookieOptions rememberOptions = new()
    {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(7),
        Path = "/",
        Secure = true,
        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None
    };






    // Making requests

    public static HttpClient AddBearerHeader(this HttpClient client, HttpContext context)
    {
        string? accessToken = context.GetAccessToken();
        if (accessToken is null)
            return client;

        AddBearerHeader(client, accessToken);
        return client;
    }

    public static HttpClient AddBearerHeader(this HttpClient client, string accessToken)
    {
        client.DefaultRequestHeaders.Remove("Cookie");
        CookieHeaderValue cookie = new(BEARER_COOKIE_NAME, accessToken);
        client.DefaultRequestHeaders.AddTokenHeader(cookie.ToString());
        return client;
    }

    public static HttpClient AddRefreshHeader(this HttpClient client, string refreshToken)
    {
        // No removing
        CookieHeaderValue cookie = new(REFRESH_COOKIE_NAME, refreshToken);
        client.DefaultRequestHeaders.AddTokenHeader(cookie.ToString());
        return client;
    }

    private static void AddTokenHeader(this HttpRequestHeaders header, string cookie)
    {
        header.Add("Cookie", cookie);
    }






    // Storing data for next use

    public static void SetAuthCookies(this HttpResponse response, AuthDto dto)
    {
        SetAuthCookie(response, BEARER_COOKIE_NAME, dto.AccessToken);
        SetAuthCookie(response, REFRESH_COOKIE_NAME, dto.RefreshToken);
    }

    public static void SetAuthCookie(this HttpResponse response, string name, string value)
    {
        response.Cookies.Append(name, value, credentialOptions);
    }

    public static void ExpireAuthCookies(this HttpResponse response)
    {
        response.Cookies.Append(BEARER_COOKIE_NAME, string.Empty, expiredOptions);
        response.Cookies.Append(REFRESH_COOKIE_NAME, string.Empty, expiredOptions);
    }

    public static void SetClientData(this HttpContext context, string sData)
        => context.Session.SetString(SESSION_USERINFO, sData);

    public static void AddRememberCookie(this HttpResponse response, Guid value)
        => response.Cookies.Append(REMEMBER_COOKIE_NAME, value.ToString(), rememberOptions);






    // Retrieving data

    public static bool IsAuthenticated(this HttpContext context)
    {
        if (context.User.Identity is null)
            return false;
        return context.User.Identity.IsAuthenticated;
    }

    public static string? GetAccessToken(this HttpContext context)
        => context.Request.Cookies[BEARER_COOKIE_NAME];

    public static string? GetRefreshToken(this HttpContext context)
        => context.Request.Cookies[REFRESH_COOKIE_NAME];

    private static string? GetRememberCookie(this HttpContext context)
        => context.Request.Cookies[REMEMBER_COOKIE_NAME];

    public static async Task<UserFullModel?> GetClientData(this HttpContext context)
    {
        string? s_userInfo = context.Session.GetString(SESSION_USERINFO);

        if (s_userInfo is null && GetRememberCookie(context) is not null)
        {
            string? accessToken = context.GetAccessToken();
            if (accessToken is null)
                return null;
            var client = GetTemporaryClient(accessToken);
            var response = await client.GetAsync("api/users/client");
            string sResponse = await response.Content.ReadAsStringAsync();
            context.SetClientData(sResponse);
            return JsonSerializer.Deserialize<UserFullModel>(sResponse)!;
        }
        return s_userInfo is not null
            ? JsonSerializer.Deserialize<UserFullModel>(s_userInfo)
            : null;
    }






    public static HttpClient GetTemporaryClient(string accessToken, string? refreshToken = null)
    {
        ApiClientOptions options = Configurer.GetApiClientOptions();
        HttpClient client = new() { BaseAddress = new Uri(options.ApiServerPath) };

        client.AddBearerHeader(accessToken);
        if (refreshToken is not null)
            client.AddRefreshHeader(refreshToken);

        return client;
    }
}
