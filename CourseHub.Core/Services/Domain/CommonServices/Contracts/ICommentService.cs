using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using CourseHub.Core.RequestDtos.Common.CommentDtos;

namespace CourseHub.Core.Services.Domain.CommonServices.Contracts;

public interface ICommentService
{
    Task<ServiceResult<PagedResult<CommentModel>>> GetPagedAsync(QueryCommentDto dto);

    Task<ServiceResult<Guid>> CreateAsync(CreateCommentDto dto, Guid? client);
    Task<ServiceResult> UpdateAsync(UpdateCommentDto dto, Guid? client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid? client);
}
