using AutoMapper;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Services.Mappers.UserMappers;

public class UserMapperProfile : Profile
{
    public static readonly IConfigurationProvider FullModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<User, UserFullModel>()
    );

    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<User, UserModel>()
    );

    public static readonly IConfigurationProvider OverviewModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<User, UserOverviewModel>()
    );

    public static readonly IConfigurationProvider MinModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<User, UserMinModel>()
    );

    public UserMapperProfile()
    {
        CreateMap<User, UserMinModel>();
        CreateMap<User, UserFullModel>();
    }
}
