using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.RequestDtos.Common.CommentDtos;

namespace CourseHub.Core.Services.Domain.CommonServices;

public interface ICommentService
{
    Task<ServiceResult<PagedResult<Comment>>> GetPagedAsync(QueryCommentDto dto);

    Task<ServiceResult<Guid>> CreateAsync(CreateCommentDto dto, Guid? client);
    Task<ServiceResult> UpdateAsync(UpdateCommentDto dto, Guid? client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid? client);
}
