using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Social.ChatMessageDtos;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts.SocialServices;

namespace CourseHub.UI.Services.Implementations.SocialServices;

public class ChatMessageApiService : IChatMessageApiService
{
    private readonly HttpClient _client;

    public ChatMessageApiService(HttpClient client)
    {
        _client = client;
    }



    public async Task<PagedResult<ChatMessageModel>> GetAsync(QueryChatMessageDto dto, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<PagedResult<ChatMessageModel>>(
                $"api/chatmessages?{QueryBuilder.Build(dto)}", SerializeOptions.JsonOptions);

            return result;
        }
        catch
        {
            return PagedResult<ChatMessageModel>.GetEmpty();
        }
    }
}
