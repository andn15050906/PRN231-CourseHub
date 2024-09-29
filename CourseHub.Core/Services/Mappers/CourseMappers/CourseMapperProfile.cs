using AutoMapper;
using CourseHub.Core.Models.Course.CourseModels;

namespace CourseHub.Core.Services.Mappers.CourseMappers;

public class CourseMapperProfile : Profile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<Course, CourseModel>()
    );

    public static readonly IConfigurationProvider OverviewModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<Course, CourseOverviewModel>()
    );

    public static readonly IConfigurationProvider MinModelConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<Course, CourseMinModel>()
    );
}
