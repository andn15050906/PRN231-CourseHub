using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;
using CourseHub.Core.Services.Domain.AssignmentServices.Contracts;

namespace CourseHub.Core.Services.Domain.AssignmentServices;

public class AssignmentService : DomainService, IAssignmentService
{
    public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }



    public async Task<ServiceResult<AssignmentModel>> GetAsync(Guid id)
    {
        var result = await _uow.AssignmentRepo.GetAsync(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<AssignmentMinModel>> GetMinAsync(Guid id)
    {
        var result = await _uow.AssignmentRepo.GetMinAsync(id);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<AssignmentMinModel>>> GetBySectionsAsync(IEnumerable<Guid> sections)
    {
        var result = await _uow.AssignmentRepo.GetBySectionsAsync(sections);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<AssignmentMinModel>>> GetByCourseAsync(Guid courseId)
    {
        var course = await _uow.CourseRepo.GetAsync(courseId);
        if (course is null)
            return NotFound<List<AssignmentMinModel>>();

        var result = await _uow.AssignmentRepo.GetBySectionsAsync(course.Sections.Select(_ => _.Id));
        return ToQueryResult(result);
    }






    public async Task<ServiceResult<Guid>> CreateAsync(CreateAssignmentDto dto, Guid client)
    {
        var user = await _uow.UserRepo.Find(client);
        if (user is null)
            return Unauthorized<Guid>();

        var entity = Adapt(dto, user);
        await _uow.AssignmentRepo.Insert(entity);
        await _uow.CommitAsync();
        return Created(entity.Id);
    }

    public async Task<ServiceResult> UpdateAsync(UpdateAssignmentDto dto, Guid client)
    {
        var entity = await _uow.AssignmentRepo.Find(dto.Id);
        if (entity is null)
            return NotFound();
        if (entity.CreatorId != client)
            return Unauthorized();

        ApplyChanges(entity, dto);
        await _uow.CommitAsync();
        return Ok();
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, Guid client)
    {
        var entity = await _uow.AssignmentRepo.Find(id);
        if (entity is null)
            return NotFound();
        if (entity.CreatorId != client)
            return Forbidden();

        _uow.AssignmentRepo.Delete(entity);
        await _uow.CommitAsync();
        return Ok();
    }






    private Assignment Adapt(CreateAssignmentDto dto, User creator)
    {
        Guid assignmentId = Guid.NewGuid();

        var questions = dto.Questions.Select(_ =>
            new McqQuestion(
                _.Content,
                assignmentId,
                _.Choices.Select(_ => new McqChoice(_.Content, _.IsCorrect)).ToList()
            )
        ).ToList();
        return new Assignment(assignmentId, dto.Name, dto.Duration, dto.GradeToPass, dto.SectionId, creator, questions);
    }

    private void ApplyChanges(Assignment entity, UpdateAssignmentDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Name))
            entity.Name = dto.Name;
        if (dto.Duration is not null)
            entity.Duration = (int) dto.Duration;

        /*if (dto.Questions is not null)
        {
            var questions = dto.Questions.Select(_ =>
                new McqQuestion(
                    _.Content,
                    entity.Id,
                    _.Choices.Select(_ => new McqChoice(_.Content, _.IsCorrect)).ToList()
                )
            ).ToList();

            entity.Questions = questions;
        }*/

        if (dto.GradeToPass is not null)
            entity.GradeToPass = (double)dto.GradeToPass;
    }
}
