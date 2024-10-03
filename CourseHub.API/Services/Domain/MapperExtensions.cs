using CourseHub.Core.Services.Mappers.ConversationMappers;
using CourseHub.Core.Services.Mappers.CourseMappers;
using CourseHub.Core.Services.Mappers.UserMappers;

namespace CourseHub.API.Services.Domain;

public static class MapperExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(UserMapperProfile),
            typeof(CourseMapperProfile),

            typeof(ChatMessageMapperProfile)
        );

        return services;
    }
}
