using CourseHub.API.Helpers.AppStart;
using CourseHub.Core.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;

namespace CourseHub.API.Services.Authentication;

public static class AuthenticationExtensions
{
    private const string SCHEME = "Scheme";

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        TokenOptions tokenConfig = Configurer.GetTokenOptions();
        OAuthOptions googleConfig = Configurer.GetGoogleOAuthOptions();

        services.Configure<TokenOptions>(options =>
        {
            options.Issuer = tokenConfig.Issuer;
            options.Audience = tokenConfig.Audience;
            options.Secret = tokenConfig.Secret;
            options.Lifetime = tokenConfig.Lifetime;
        });

        services.AddTransient<ITokenService, TokenService>();

        services
            .AddAuthentication(options => {
                options.DefaultScheme = SCHEME;
            })
            .AddPolicyScheme(SCHEME, null, options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    return context.Request.Cookies[Helpers.Cookie.CookieExtensions.BEARER] is not null
                        ? JwtBearerDefaults.AuthenticationScheme
                        : CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddJwtBearer(BearerOptionsProvider.GetOptions(tokenConfig))
            .AddCookie(options =>
            {
                //options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.IsEssential = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = googleConfig.ClientId;
                options.ClientSecret = googleConfig.ClientSecret;

                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                //options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
            });

        return services;
    }
}
