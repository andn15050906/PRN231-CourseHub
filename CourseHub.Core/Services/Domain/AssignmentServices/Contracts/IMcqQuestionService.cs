using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Models.Assignment.McqQuestionModels;
using CourseHub.Core.RequestDtos.Assignment.McqQuestionDtos;

namespace CourseHub.Core.Services.Domain.AssignmentServices.Contracts;

public interface IMcqQuestionService
{
    Task<List<McqQuestionModel>> GetByAssignment(Guid assignmentId);
    Task<ServiceResult<Guid>> Create(CreateMcqQuestionDto dto);
    Task<ServiceResult> Update(UpdateMcqQuestionDto dto);
    Task<ServiceResult> Delete(Guid id);
}
