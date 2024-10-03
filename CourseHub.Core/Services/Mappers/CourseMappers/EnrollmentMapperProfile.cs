using AutoMapper;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Services.Mappers.CourseMappers;

public class EnrollmentMapperProfile : Profile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<User, UserMinModel>();
            cfg.CreateMap<Course, CourseOverviewModel>();

            cfg.CreateMap<Enrollment, EnrollmentModel>();
        }
    );

    public static readonly IConfigurationProvider FullModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<User, UserMinModel>();
            cfg.CreateMap<Course, CourseOverviewModel>();

            cfg.CreateMap<Enrollment, EnrollmentFullModel>();
        }
    );
}
