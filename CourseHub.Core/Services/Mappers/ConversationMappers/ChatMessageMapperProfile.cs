using AutoMapper;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Course.CourseDtos;

namespace CourseHub.Core.Services.Mappers.ConversationMappers;

public class ChatMessageMapperProfile : Profile
{
    public static readonly IConfigurationProvider ModelConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.CreateMap<ChatMessage, ChatMessageModel>();
        }
    );

    public ChatMessageMapperProfile()
    {
        CreateMap<ChatMessage, ChatMessageModel>();
    }
}
