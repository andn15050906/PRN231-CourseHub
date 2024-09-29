using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;

namespace CourseHub.Core.Services.Domain.CourseServices;

public interface ICourseService
{
    Task<ServiceResult<CourseModel>> GetAsync(Guid id);
    Task<ServiceResult<PagedResult<CourseOverviewModel>>> GetPagedAsync(QueryCourseDto dto);
    Task<ServiceResult<PagedResult<CourseOverviewModel>>> GetMultiple(Guid[] ids);
    Task<ServiceResult<List<CourseOverviewModel>>> GetSimilar(Guid id);
    Task<ServiceResult<List<CourseMinModel>>> GetMin(QueryCourseDto id);

    Task<ServiceResult<Guid>> CreateAsync(CreateCourseDto dto, Guid? client);
    Task<ServiceResult> UpdateAsync(UpdateCourseDto dto, Guid? client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid? client);
}
