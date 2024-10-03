using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseReviewModels;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;

namespace CourseHub.UI.Services.Contracts.CourseServices;

public interface ICourseReviewApiService
{
    Task<PagedResult<CourseReviewModel>> GetAsync(QueryCourseReviewDto dto);
    Task<HttpResponseMessage> CreateAsync(CreateCourseReviewDto dto, HttpContext context);
    Task<HttpResponseMessage> UpdateAsync(UpdateCourseReviewDto dto, HttpContext context);
    Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context);
}
