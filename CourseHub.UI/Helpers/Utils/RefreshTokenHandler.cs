using CourseHub.Core.Services.Domain.UserServices.TempModels;
using CourseHub.UI.Helpers.AppStart;
using System.Net;

namespace CourseHub.UI.Helpers.Utils;

public class RefreshTokenHandler : DelegatingHandler
{
    private const string API_ROUTE = "api/auth/refresh";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenHandler(IHttpContextAccessor httpContextAccessor) : base()
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (!IsBearerExpired(response, cancellationToken))
            return response;

        var result = await TryGetNewAuthInfo(_httpContextAccessor.HttpContext, cancellationToken);
        if (result.Item1 is null || result.Item2 is null)
            return response;

        request = new HttpRequestMessage(request.Method, request.RequestUri);
        response = await result.Item2.SendAsync(request, cancellationToken);
        return response;
    }

    private bool IsBearerExpired(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return false;
        if (!response.Headers.TryGetValues("WWW-Authenticate", out IEnumerable<string>? values))
            return false;
        if (!values.Any(_ => _.Contains("invalid_token")))
            return false;
        return true;
    }

    private async Task<(AuthDto?, HttpClient?)> TryGetNewAuthInfo(HttpContext? context, CancellationToken cancellationToken)
    {
        if (context is null)
            return (null, null);
        string? refreshToken = context.GetRefreshToken();
        if (string.IsNullOrEmpty(refreshToken))
            return (null, null);
        string? accessToken = context.GetAccessToken();
        if (string.IsNullOrEmpty(accessToken))
            return (null, null);

        var options = Configurer.GetApiClientOptions();
        HttpClient client = new() { BaseAddress = new Uri(options.ApiServerPath) };
        client.AddBearerHeader(accessToken);
        client.AddRefreshHeader(refreshToken);
        var refreshRequest = new HttpRequestMessage(HttpMethod.Post, API_ROUTE);
        var refreshResponse = await client.SendAsync(refreshRequest, cancellationToken);

        if (!refreshResponse.IsSuccessStatusCode)
            return (null, null);
        AuthDto? authData = await refreshResponse.Content.ReadFromJsonAsync<AuthDto>(cancellationToken: cancellationToken);
        
        if (authData is not null)
            context.Response.SetAuthCookies(authData);
        return (authData, client);
    }
}
