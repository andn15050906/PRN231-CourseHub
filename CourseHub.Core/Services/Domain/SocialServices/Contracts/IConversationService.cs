using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Social;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;

namespace CourseHub.Core.Services.Domain.SocialServices.Contracts;

public interface IConversationService
{
    Task<ServiceResult<ConversationModel>> Get(Guid id, Guid? client);
    Task<ServiceResult<PagedResult<ConversationModel>>> Get(QueryConversationDto dto, Guid? client);
    Task<ServiceResult<List<ConversationModel>>> GetMultiple(List<Guid> ids);
    Task<ServiceResult<PagedResult<ConversationModel>>> GetConversationsOrUsers(QueryConversationDto dto, Guid? client);

    Task<ServiceResult<Guid>> Create(CreateConversationDto dto, Guid client);
    Task<ServiceResult> Update(UpdateConversationDto dto, Guid client);
    Task<ServiceResult> Delete(Guid id, Guid client);
}
