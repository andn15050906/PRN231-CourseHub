using CourseHub.Core.Services.Mappers.UserMappers;

namespace CourseHub.API.Services.Domain;

public static class MapperExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(UserMapperProfile)
        );

        return services;
    }
}
