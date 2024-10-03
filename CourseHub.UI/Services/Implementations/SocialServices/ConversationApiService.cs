using CourseHub.Core.Helpers.Http;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Cache;
using CourseHub.UI.Services.Contracts.SocialServices;

namespace CourseHub.UI.Services.Implementations.SocialServices;

public class ConversationApiService : IConversationApiService
{
    private readonly HttpClient _client;
	//private readonly CacheService _cache;

	public ConversationApiService(HttpClient client/*, CacheService cache*/)
    {
        _client = client;
		//_cache = cache;
	}



    public async Task<PagedResult<ConversationModel>> GetAsync(QueryConversationDto dto, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<PagedResult<ConversationModel>>(
                $"api/conversations?{QueryBuilder.Build(dto)}", SerializeOptions.JsonOptions);

            foreach (var item in result!.Items)
            {
                if (string.IsNullOrEmpty(item.AvatarUrl))
                    continue;
                if (!ResourceHelper.IsRemote(item.AvatarUrl))
                    item.AvatarUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/conversations/Resource/{item.Id}";
            }

			/*foreach (var conversation in result.Items)
				_cache.SetConversation(conversation);*/
			return result;
        }
        catch
        {
            return PagedResult<ConversationModel>.GetEmpty();
        }
    }

    public async Task<ConversationModel?> GetAsync(Guid id, HttpContext context)
    {
        ConversationModel? result;
        /*_cache.TryGetConversation(id, out var result);
        if (result is not null)
            return result;*/

        try
        {
            _client.AddBearerHeader(context);
            result = await _client.GetFromJsonAsync<ConversationModel>(
                $"api/conversations/{id}", SerializeOptions.JsonOptions);

            if (string.IsNullOrEmpty(result!.AvatarUrl))
            {

            }
            else if (!ResourceHelper.IsRemote(result.AvatarUrl))
			{
				result.AvatarUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/conversations/Resource/{result.Id}";
			}

            /*_cache.SetConversation(result);*/
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<ConversationModel>> GetMultipleAsync(IEnumerable<Guid> ids, HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);

            string url = QueryBuilder.BuildWithArray("api/conversations/multiple?", "ids={0}&", ids.Select(_ => _.ToString()));
            var result = await _client.GetFromJsonAsync<List<ConversationModel>>(url, SerializeOptions.JsonOptions);

            foreach (var item in result!)
                if (!ResourceHelper.IsRemote(item.AvatarUrl))
                    item.AvatarUrl = Configurer.GetApiClientOptions().ApiServerPath + $"/api/conversations/Resource/{item.Id}";

            return result;
        }
        catch
        {
            return new();
        }
    }

    public async Task<HttpResponseMessage> CreateAsync(CreateConversationDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PostAsync("/api/conversations", ToFormData(dto));
        return result;
    }

    public async Task<HttpResponseMessage> UpdateAsync(UpdateConversationDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.PatchAsync("/api/conversations", ToFormData(dto));

        //_cache.RemoveConversation(dto.Id);
        return result;
    }

    public async Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var result = await _client.DeleteAsync($"/api/conversations/{id}");

		//_cache.RemoveConversation(id);
		return result;
    }






    private MultipartFormDataContent ToFormData(CreateConversationDto dto)
    {
        FormDataHelper helper = new()
        {
            KeyValuePairs = new()
            {
                { nameof(dto.Title), dto.Title },
                { "Avatar.Url", dto.Avatar?.Url }
            }
        };

        if (dto.OtherParticipants is not null && dto.OtherParticipants.Count > 0)
        {
            for (int i = 0; i < dto.OtherParticipants.Count; i++)
                helper.KeyValuePairs.Add($"OtherParticipants[{i}]", dto.OtherParticipants[i].ToString());
        }

        if (dto.Avatar?.File is not null)
        {
            helper.Files = new List<(Stream, string, string)>
            {
                (dto.Avatar.File.OpenReadStream(), "Avatar.File", dto.Avatar.File.FileName)
            };
        }

        return helper.ToFormData();
    }

    private MultipartFormDataContent ToFormData(UpdateConversationDto dto)
    {
        FormDataHelper helper = new()
        {
            KeyValuePairs = new()
            {
                { nameof(dto.Id), dto.Id.ToString() },
                { nameof(dto.Title), dto.Title },
                { "Avatar.Url", dto.Avatar?.Url }

                //...
            }
        };

        if (dto.Avatar?.File is not null)
        {
            helper.Files = new List<(Stream, string, string)>
            {
                (dto.Avatar.File.OpenReadStream(), "Avatar.File", dto.Avatar.File.FileName)
            };
        }

        return helper.ToFormData();
    }
}
