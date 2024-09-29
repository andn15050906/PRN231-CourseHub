using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseReviewModels;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;

namespace CourseHub.Core.Services.Domain.CourseServices.Contracts;

public interface ICourseReviewService
{
    Task<ServiceResult<PagedResult<CourseReviewModel>>> GetPagedAsync(QueryCourseReviewDto dto);

    Task<ServiceResult<Guid>> CreateAsync(CreateCourseReviewDto dto, Guid? client);
    Task<ServiceResult> UpdateAsync(UpdateCourseReviewDto dto, Guid? client);
}
