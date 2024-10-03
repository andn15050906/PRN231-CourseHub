using AutoMapper;
using CourseHub.Core.Models.Common.CommentModels;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Services.Mappers.CommonMappers;

public class CommentMapperProfile : Profile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<User, UserMinModel>();
            cfg.CreateMap<Comment, CommentModel>();
        }
    );
}
