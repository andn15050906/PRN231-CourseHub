using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Social.ChatMessageDtos;

namespace CourseHub.UI.Services.Contracts.SocialServices;

public interface IChatMessageApiService
{
    Task<PagedResult<ChatMessageModel>> GetAsync(QueryChatMessageDto dto, HttpContext context);
}
