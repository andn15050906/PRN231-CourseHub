using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using System.Linq.Expressions;

namespace CourseHub.Core.Services.Domain.CourseServices;

internal class CourseService : DomainService, ICourseService
{
    public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }






    public async Task<ServiceResult<CourseModel>> GetAsync(Guid id)
    {
        var result = await _uow.CourseRepo.GetAsync(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<CourseMinModel>>> GetMin(QueryCourseDto id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<PagedResult<CourseOverviewModel>>> GetMultiple(Guid[] ids)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<PagedResult<CourseOverviewModel>>> GetPagedAsync(QueryCourseDto dto)
    {
        var query = _uow.CourseRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize);
        var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<CourseOverviewModel>>> GetSimilar(Guid id)
    {
        var result = await _uow.CourseRepo.GetSimilar(id);
        return ToQueryResult(result);
    }






    public Task<ServiceResult<Guid>> CreateAsync(CreateCourseDto dto, Guid? client)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> UpdateAsync(UpdateCourseDto dto, Guid? client)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> DeleteAsync(Guid id, Guid? client)
    {
        throw new NotImplementedException();
    }






    private Expression<Func<Course, bool>>? GetPredicate(QueryCourseDto dto)
    {
        return null;
    }
}
