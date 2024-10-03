using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Models.Assignment.McqQuestionModels;
using CourseHub.Core.RequestDtos.Assignment.McqQuestionDtos;
using CourseHub.Core.Services.Domain.AssignmentServices.Contracts;

namespace CourseHub.Core.Services.Domain.AssignmentServices;

public class McqQuestionService : DomainService, IMcqQuestionService
{
    public McqQuestionService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }



    public Task<List<McqQuestionModel>> GetByAssignment(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<Guid>> Create(CreateMcqQuestionDto dto)
    {
        if (dto.AssignmentId is null || dto.AssignmentId == default)
            return BadRequest<Guid>(AssignmentDomainMessages.INVALID_ASSIGNMENT);

        var entity = Adapt(dto);
        await _uow.McqQuestionRepo.Insert(entity);
        await _uow.CommitAsync();
        return Created(entity.Id);
    }

    public Task<ServiceResult> Update(UpdateMcqQuestionDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> Delete(Guid id)
    {
        throw new NotImplementedException();
    }



    private McqQuestion Adapt(CreateMcqQuestionDto dto)
    {
        var choiceList = dto.Choices.Select(_ => new McqChoice(_.Content, _.IsCorrect)).ToList();
        return new McqQuestion(dto.Content, (Guid)dto.AssignmentId!, choiceList);
    }
}
