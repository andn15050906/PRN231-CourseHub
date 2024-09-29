using AutoMapper;
using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.Models.Assignment.McqQuestionModels;
using CourseHub.Core.Models.Course.SectionModels;

namespace CourseHub.Core.Services.Mappers.AssignmentMappers;

public class AssignmentMapperProfile : Profile
{
    public static readonly IConfigurationProvider MinModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<Section, SectionMinModel>();
            cfg.CreateMap<Assignment, AssignmentMinModel>();
        }
    );

    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<Section, SectionMinModel>();
            cfg.CreateMap<McqQuestion, McqQuestionModel>();
            cfg.CreateMap<Assignment, AssignmentModel>();
        }
    );
}
