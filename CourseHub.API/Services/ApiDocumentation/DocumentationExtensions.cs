using Microsoft.OpenApi.Models;

namespace CourseHub.API.Services.ApiDocumentation;

public static class DocumentationExtensions
{
    private const string VERSION = "v1";
    private const string TITLE = "CourseHub's APIs";
    private const string DESCRIPTION = "You are viewing the API documentation of CourseHub";

    private const string SECURITY_DEFINITION_NAME = "Bearer";
    private const string SECURITY_SCHEME = SECURITY_DEFINITION_NAME;
    private const SecuritySchemeType SECURITY_SCHEME_TYPE = SecuritySchemeType.Http;
    private const string SECURITY_SCHEME_NAME = "Authorization";
    private const string SECURITY_SCHEME_DESCRIPTION = "Enter your JWT Bearer";
    private const ParameterLocation SECURITY_LOCATION = ParameterLocation.Cookie;



    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options => {
                options.SwaggerDoc(VERSION, new OpenApiInfo
                {
                    Version = VERSION,
                    Title = TITLE,
                    Description = DESCRIPTION
                });
                options.AddSecurityDefinition(SECURITY_DEFINITION_NAME, new OpenApiSecurityScheme
                {
                    Scheme = SECURITY_SCHEME,
                    Name = SECURITY_SCHEME_NAME,
                    Description = SECURITY_SCHEME_DESCRIPTION,
                    In = SECURITY_LOCATION,
                    Type = SECURITY_SCHEME_TYPE
                });
            });
        return services;
    }

    public static void UseDocumentation(this WebApplication app)
    {
        app.UseSwagger().UseSwaggerUI();
    }
}
