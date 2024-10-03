using AutoMapper;
using CourseHub.Core.Models.Common.NotificationModels;

namespace CourseHub.Core.Services.Mappers.CommonMappers;

public class NotificationMapperProfile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<Notification, NotificationModel>()
    );
}
