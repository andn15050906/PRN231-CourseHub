using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;

namespace CourseHub.Core.Services.Domain.CourseServices.Contracts;

public interface ICourseService
{
    Task<ServiceResult<CourseModel>> GetAsync(Guid id);
    Task<ServiceResult<PagedResult<CourseOverviewModel>>> GetPagedAsync(QueryCourseDto dto);
    Task<ServiceResult<PagedResult<CourseMinModel>>> GetMinAsync(QueryCourseDto dto);
    Task<ServiceResult<CourseMinModel>> GetMinAsync(Guid id);
    Task<ServiceResult<CourseOverviewModel>> GetBySectionAsync(Guid sectionId);
    Task<ServiceResult<List<CourseOverviewModel>>> GetMultipleAsync(Guid[] ids);
    Task<ServiceResult<List<CourseOverviewModel>>> GetSimilarAsync(Guid id);

    Task<ServiceResult<Guid>> CreateAsync(CreateCourseDto dto, Guid client);
    Task<ServiceResult> UpdateAsync(UpdateCourseDto dto, Guid client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid client);
}
