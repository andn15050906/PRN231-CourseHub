using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;

namespace CourseHub.UI.Services.Contracts.SocialServices;

public interface IConversationApiService
{
    Task<PagedResult<ConversationModel>> GetAsync(QueryConversationDto dto, HttpContext context);
    Task<ConversationModel?> GetAsync(Guid id, HttpContext context);
    Task<List<ConversationModel>> GetMultipleAsync(IEnumerable<Guid> ids, HttpContext context);

    Task<HttpResponseMessage> CreateAsync(CreateConversationDto dto, HttpContext context);
    Task<HttpResponseMessage> UpdateAsync(UpdateConversationDto dto, HttpContext context);
    Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context);
}
