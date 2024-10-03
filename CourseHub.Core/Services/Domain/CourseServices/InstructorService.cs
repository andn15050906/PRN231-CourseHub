using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;

namespace CourseHub.Core.Services.Domain.CourseServices;

public class InstructorService : DomainService, IInstructorService
{
    public InstructorService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }



    public async Task<ServiceResult<PagedResult<InstructorModel>>> GetAsync(QueryInstructorDto dto)
    {
        var query =  _uow.InstructorRepo.GetPagingQuery(_ => true, 0, 20);
        var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<InstructorModel>> GetByUserIdAsync(Guid userId)
    {
        var result = await _uow.InstructorRepo.GetByUserIdAsync(userId);
        return ToQueryResult(result);
    }

    /// <summary>
    /// Not committed
    /// </summary>
    /*public async Task<ServiceResult<Guid>> CreateAsync(CreateInstructorDto dto, Guid creatorId)
    {
        var entity = Adapt(dto, creatorId);
        await _uow.InstructorRepo.Insert(entity);
        return Created(entity.Id);
    }*/

    public async Task<ServiceResult> UpdateAsync(UpdateInstructorDto dto, Guid client)
    {
        var entity = await _uow.InstructorRepo.FindEntityByUserIdAsync(client);

        if (entity is null)
            return Unauthorized();

        ApplyChanges(dto, entity);
        await _uow.CommitAsync();
        return Ok();
    }






    private Instructor Adapt(CreateInstructorDto dto, Guid creatorId)
    {
        return new Instructor(Guid.NewGuid(), creatorId, dto.Intro, dto.Experience);
    }

    private void ApplyChanges(UpdateInstructorDto dto, Instructor entity)
    {
        if (dto.Intro is not null)
            entity.Intro = dto.Intro;
        if (dto.Experience is not null)
            entity.Experience = dto.Experience;
    }
}
