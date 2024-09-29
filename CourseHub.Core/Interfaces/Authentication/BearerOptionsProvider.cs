using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CourseHub.Core.Interfaces.Authentication;

public static class BearerOptionsProvider
{
    private const string BEARER_COOKIE_NAME = "Bearer";

    public static Action<JwtBearerOptions> GetOptions(TokenOptions config) => options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = config.Audience,
            ValidIssuer = config.Issuer,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[BEARER_COOKIE_NAME];
                return Task.CompletedTask;
            }
        };
    };
}
