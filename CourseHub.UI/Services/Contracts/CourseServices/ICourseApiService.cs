using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;

namespace CourseHub.UI.Services.Contracts.CourseServices;

public interface ICourseApiService
{
    Task<CourseModel?> GetAsync(Guid id);
    Task<CourseOverviewModel?> GetBySectionIdAsync(Guid sectionId);
    Task<PagedResult<CourseOverviewModel>> GetPagedAsync(QueryCourseDto dto);
    Task<List<CourseOverviewModel>?> GetMultipleAsync(IEnumerable<Guid> ids);
    Task<List<CourseOverviewModel>?> GetSimilarAsync(Guid id);
    Task<PagedResult<CourseMinModel>?> GetMinAsync(QueryCourseDto id);

    Task<HttpResponseMessage> CreateAsync(CreateCourseDto dto, HttpContext context);
    Task<HttpResponseMessage> UpdateAsync(UpdateCourseDto dto, HttpContext context);
    Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context);

    Task<bool> IsEnrolled(Guid courseId, HttpContext context);
    Task<List<EnrollmentModel>> GetEnrollmentsAsync(HttpContext context);
    Task<EnrollmentFullModel?> GetEnrollmentAsync(HttpContext context, Guid courseId);
    Task<HttpResponseMessage> GrantEnrollmentAsync(Guid courseId, Guid userId, HttpContext context);
}