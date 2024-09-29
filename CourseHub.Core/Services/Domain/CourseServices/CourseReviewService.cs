using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseReviewModels;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using System.Linq.Expressions;

namespace CourseHub.Core.Services.Domain.CourseServices;

public class CourseReviewService : DomainService, ICourseReviewService
{
    public CourseReviewService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }

    public async Task<ServiceResult<PagedResult<CourseReviewModel>>> GetPagedAsync(QueryCourseReviewDto dto)
    {
        var query = _uow.CourseReviewRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize);
        var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime, ascending: false);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateCourseReviewDto dto, Guid? client)
    {
        if (client is null)
            return Unauthorized<Guid>();
        var author = await _uow.UserRepo.Find(client);
        if (author is null)
            return Unauthorized<Guid>();

        var entity = Adapt(dto, author);
        try
        {
            await _uow.CourseReviewRepo.Insert(entity);
            await _uow.CommitAsync();
            return Created(entity.Id);
        }
        catch
        {
            return ServerError<Guid>();
        }
    }

    public async Task<ServiceResult> UpdateAsync(UpdateCourseReviewDto dto, Guid? client)
    {
        if (client is null)
            return Unauthorized();
        var author = await _uow.UserRepo.Find(client);
        if (author is null)
            return Unauthorized();

        var entity = await _uow.CourseReviewRepo.Find(dto.Id);
        if (entity is null)
            return NotFound();
        ApplyChanges(dto, entity);
        try
        {
            await _uow.CommitAsync();
            return Ok();
        }
        catch
        {
            return ServerError();
        }
    }






    private Expression<Func<CourseReview, bool>>? GetPredicate(QueryCourseReviewDto query)
    {
        return _ => _.CourseId == query.CourseId;
    }

    private CourseReview Adapt(CreateCourseReviewDto dto, User author)
    {
        return new CourseReview(dto.Content, dto.Rating, dto.CourseId, author);
    }

    private void ApplyChanges(UpdateCourseReviewDto dto, CourseReview entity)
    {
        //...
        //entity.Rating = dto.Rating;
        entity.Content = dto.Content;
        entity.LastModificationTime = DateTime.UtcNow;
    }
}
