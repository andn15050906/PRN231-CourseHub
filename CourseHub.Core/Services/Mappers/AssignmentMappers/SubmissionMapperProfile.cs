using AutoMapper;
using CourseHub.Core.Models.Assignment.SubmissionModels;

namespace CourseHub.Core.Services.Mappers.AssignmentMappers;

public class SubmissionMapperProfile : Profile
{
    public static readonly IConfigurationProvider MinModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<Submission, SubmissionMinModel>();
        }
    );

    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<Submission, SubmissionModel>();
        }
    );
}
