using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Text;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using CourseHub.Core.Services.Storage;
using CourseHub.Core.Services.Storage.Utils;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace CourseHub.Core.Services.Domain.CourseServices;

public class CourseService : DomainService, ICourseService
{
    public CourseService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }






    public async Task<ServiceResult<CourseModel>> GetAsync(Guid id)
    {
        var result = await _uow.CourseRepo.GetAsync(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<PagedResult<CourseOverviewModel>>> GetPagedAsync(QueryCourseDto dto)
    {
        var query = _uow.CourseRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize, GetInclude(dto));

        PagedResult<CourseOverviewModel> result;
        if (dto.ByPrice is true)
            result = await query.ExecuteWithOrderBy(_ => _.Price);
        else if (dto.ByDiscount is true)
            result = await query.ExecuteWithOrderBy(_ => _.Discount, ascending: false);
        else if (dto.ByLearnerCount is true)
            result = await query.ExecuteWithOrderBy(_ => _.LearnerCount, ascending: false);
        else if (dto.ByAvgRating is true)
            result = await query.ExecuteWithOrderBy(_ => _.TotalRating / _.RatingCount, ascending: false, isAnsiWarningTransaction: true);
        else
            result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime, ascending: false);



        return ToQueryResult(result);
    }

    public async Task<ServiceResult<PagedResult<CourseMinModel>>> GetMinAsync(QueryCourseDto dto)
    {
        var query = _uow.CourseRepo.GetPagingQuery(GetPredicate(dto), dto.PageIndex, dto.PageSize);

        PagedResult<CourseMinModel> result;
        if (dto.ByPrice is true)
            result = await query.ExecuteWithOrderBy(_ => _.Price);
        else if (dto.ByDiscount is true)
            result = await query.ExecuteWithOrderBy(_ => _.Discount, ascending: false);
        else if (dto.ByLearnerCount is true)
            result = await query.ExecuteWithOrderBy(_ => _.LearnerCount, ascending: false);
        else if (dto.ByAvgRating is true)
            result = await query.ExecuteWithOrderBy(_ => _.TotalRating / _.RatingCount, ascending: false, isAnsiWarningTransaction: true);
        else
            result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime, ascending: false);



        return ToQueryResult(result);
    }

    public async Task<ServiceResult<CourseMinModel>> GetMinAsync(Guid id)
    {
        var result = await _uow.CourseRepo.GetMinAsync(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<CourseOverviewModel>> GetBySectionAsync(Guid sectionId)
    {
        var section = await _uow.SectionRepo.GetWithCourse(sectionId);
        if (section is null)
            return BadRequest<CourseOverviewModel>(CourseDomainMessages.INVALID_SECTION);

        var result = _mapper.Map<CourseOverviewModel>(section.Course);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<CourseOverviewModel>>> GetMultipleAsync(Guid[] ids)
    {
        var result = await _uow.CourseRepo.GetMultipleAsync(ids);

        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<CourseOverviewModel>>> GetSimilarAsync(Guid id)
    {
        var result = await _uow.CourseRepo.GetSimilar(id);
        return ToQueryResult(result);
    }






    public async Task<ServiceResult<Guid>> CreateAsync(CreateCourseDto dto, Guid client)
    {
        try
        {
            var instructorId = await _uow.InstructorRepo.GetIdByUserId(client);
            if (instructorId == default)
                return ServerError<Guid>();

            var entity = await Adapt(dto, client, instructorId);
            await _uow.CourseRepo.Insert(entity);
            await _uow.CommitAsync();
            return Created(entity.Id);
        }
        catch (Exception ex)
        {
            return ServerError<Guid>(/*ex.InnerException is null ? ex.Message : ex.InnerException.Message*/);
        }
    }

    public async Task<ServiceResult> UpdateAsync(UpdateCourseDto dto, Guid client)
    {
        try
        {
            var instructorId = await _uow.InstructorRepo.GetIdByUserId(client);
            if (instructorId == default)
                return ServerError();

            var entity = await _uow.CourseRepo.Find(dto.Id);
            if (entity is null)
                return BadRequest();
            ApplyChanges(dto, entity, client);
            await _uow.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError();
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, Guid client)
    {
        var instructorId = await _uow.InstructorRepo.GetIdByUserId(client);
        if (instructorId == default)
            return ServerError();

        var entity = await _uow.CourseRepo.Find(id);
        if (entity is null)
            return NotFound();
        try
        {
            _uow.CourseRepo.Delete(entity);
            await _uow.CommitAsync();
            return Ok();
        }
        catch
        {
            return ServerError();
        }
    }






    private Expression<Func<Course, bool>>? GetPredicate(QueryCourseDto dto)
    {
        if (dto.Title is not null)
            return _ => _.MetaTitle.Contains(TextHelper.Normalize(dto.Title));
        if (dto.Status is not null)
            return _ => _.Status == dto.Status;
        if (dto.Level is not null)
            return _ => _.Level == dto.Level;
        if (dto.CategoryId is not null)
            return _ => _.LeafCategory.Path.Contains(dto.CategoryId.ToString()!);
        if (dto.InstructorId is not null)
            return _ => _.InstructorId == dto.InstructorId;
        return null;
    }

    private Expression<Func<Course, object?>>[]? GetInclude(QueryCourseDto dto)
    {
        if (dto.CategoryId is not null)
            return new Expression<Func<Course, object?>>[] { _ => _.LeafCategory };
        return null;
    }

    private async Task<Course> Adapt(CreateCourseDto dto, Guid creatorId, Guid instructorId)
    {
        Guid id = Guid.NewGuid();

        string thumbUrl = string.Empty;
        if (dto.Thumb.File is not null)
            thumbUrl = await SaveThumb(dto.Thumb.File, id);
        else if (dto.Thumb.Url is not null)
            thumbUrl = dto.Thumb.Url;

        var sections = dto.SectionNames.Select((_, index) => new Section((byte)index, _)).ToList();

        return new Course(id, creatorId, instructorId, dto.LeafCategoryId,
            dto.Title, thumbUrl, dto.Intro, dto.Description, dto.Price,
            dto.Level, dto.Outcomes, dto.Requirements, sections);
    }

    private async void ApplyChanges(UpdateCourseDto dto, Course entity, Guid client)
    {
        //... not using mapper
        if (dto.Intro is not null)
            entity.Intro = dto.Intro;
        if (dto.Description is not null)
            entity.Description = dto.Description;
        if (dto.Status is not null)
            entity.Status = (Entities.CourseDomain.Enums.CourseStatus)dto.Status;
        if (dto.Price is not null)
            entity.Price = (double)dto.Price;
        if (dto.Level is not null)
            entity.Level = (Entities.CourseDomain.Enums.CourseLevel)dto.Level;
        if (dto.Outcomes is not null)
            entity.Outcomes = dto.Outcomes;
        if (dto.Requirements is not null)
            entity.Requirements = dto.Requirements;
        if (dto.LeafCategoryId is not null)
            entity.LeafCategoryId = (Guid)dto.LeafCategoryId;


        if (dto.Title is not null)
            entity.SetTitle(dto.Title);
        if (dto.Discount is not null && dto.DiscountExpiry is not null)
            entity.SetDiscount((double)dto.Discount, (DateTime)dto.DiscountExpiry!);
        entity.LastModifierId = client;


        // Thumb
        if (dto.Thumb is not null)
        {
            if (dto.Thumb.File is not null)
                entity.ThumbUrl = await SaveThumb(dto.Thumb.File, entity.Id);
            else if (dto.Thumb.Url is not null)
                entity.ThumbUrl = dto.Thumb.Url;
        }

        // Metas
        if (dto.RemovedMetas != null)
        {
            for (int i = 0; i < entity.Metas.Count; i++)
                entity.Metas.RemoveAll(_ => dto.RemovedMetas.Any(r => r.Type == _.Type));
        }
        if (dto.AddedMetas != null)
        {
            entity.Metas.AddRange(
                dto.AddedMetas.Select(_ => new CourseMeta { Type = _.Type, Value = _.Value })
            );
        }

        // Sections
        if (dto.RemovedSections != null)
        {
            _uow.SectionRepo.RemoveRangeById(entity.Id, dto.RemovedSections);
        }
        if (dto.AddedSections != null)
        {
            _uow.CourseRepo.LoadSections(entity);
            byte currentIndex = entity.Sections.Count > 0
                ? entity.Sections.Max(_ => _.Index)
                : (byte)0;
            entity.Sections.AddRange(dto.AddedSections.Select((_, index) => new Section((byte)(currentIndex + index + 1), _)));
        }
    }

    private async Task<string> SaveThumb(IFormFile file, Guid courseId)
    {
        Stream stream = await new FileConverter().ToJpg(file);
        string path = CourseStorage.GetCourseThumbPath(courseId);
        await ServerStorage.SaveFile(stream, path, _logger);
        return path;
    }
}
