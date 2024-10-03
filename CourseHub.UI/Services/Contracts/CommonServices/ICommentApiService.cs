using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using CourseHub.Core.RequestDtos.Common.CommentDtos;

namespace CourseHub.UI.Services.Contracts.CommonServices;

public interface ICommentApiService
{
    Task<PagedResult<CommentModel>> GetAsync(QueryCommentDto dto);
    Task<HttpResponseMessage> CreateAsync(CreateCommentDto dto, HttpContext context);
    Task<HttpResponseMessage> UpdateAsync(UpdateCommentDto dto, HttpContext context);
    Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context);
}
