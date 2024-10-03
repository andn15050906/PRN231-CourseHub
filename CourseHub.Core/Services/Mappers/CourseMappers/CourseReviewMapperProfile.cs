using AutoMapper;
using CourseHub.Core.Models.Course.CourseReviewModels;

namespace CourseHub.Core.Services.Mappers.CourseMappers;

public class CourseReviewMapperProfile : Profile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<CourseReview, CourseReviewModel>();
        }
    );
}
