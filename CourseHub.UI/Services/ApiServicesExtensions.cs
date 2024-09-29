using CourseHub.Core.Services.Domain.UserServices;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts;
using CourseHub.UI.Services.Implementations;

namespace CourseHub.UI.Services;

public static class ApiServicesExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        ApiClientOptions options = Configurer.GetApiClientOptions();

        void config(HttpClient client) => client.BaseAddress = new Uri(options.ApiServerPath);
        services.AddTransient<RefreshTokenHandler>();

        services
            .AddDomainServiceWithClient<IUserApiService, UserApiService>(config);

        return services;
    }

    private static IServiceCollection AddDomainServiceWithClient<TService, TImplementation>(this IServiceCollection services, Action<HttpClient> config)
            where TService : class
            where TImplementation : class, TService
    {
        services.AddScoped<TService, TImplementation>();
        services.AddHttpClient<TService, TImplementation>(config).AddHttpMessageHandler<RefreshTokenHandler>();
        return services;
    }
}
