using AutoMapper;
using CourseHub.Core.Models.Course.InstructorModels;

namespace CourseHub.Core.Services.Mappers.CourseMappers;

public class InstructorMapperProfile : Profile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<Instructor, InstructorModel>();
        }
    );
}
