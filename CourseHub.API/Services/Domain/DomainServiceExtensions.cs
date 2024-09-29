using CourseHub.API.Helpers.AppStart;
using CourseHub.Core.Entities.UserDomain;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Services.Domain.CourseServices;
using CourseHub.Core.Services.Domain.UserServices;
using CourseHub.Infrastructure;
using CourseHub.Infrastructure.AccessContext;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.API.Services.Domain;

public static class DomainServiceExtensions
{
    private const byte RETRY_COUNT = 3;
    /*private const bool IS_SENSITIVE_DATA_LOGGING_ENABLED = true;
    private const bool IS_DETAILED_ERRORS_ENABLED = true;*/

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        string connectionString = Configurer.GetContextConnectionString();

        services.AddDbContextPool<Context>(options =>
            options.UseSqlServer(connectionString, act => act.EnableRetryOnFailure(RETRY_COUNT))
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();



        services
            .AddScoped<IUserService, UserService>()

            .AddScoped<ICategoryService, CategoryService>();



        ExecuteColdQuery(connectionString);

        return services;
    }

    private static void ExecuteColdQuery(string connectionString)
    {
        using var context = new Context(new DbContextOptionsBuilder<Context>().UseSqlServer(connectionString).Options);
        context.Set<User>().FirstOrDefault();
    }
}
