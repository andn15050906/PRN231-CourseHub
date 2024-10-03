using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Models.Assignment.SubmissionModels;
using CourseHub.Core.RequestDtos.Assignment.SubmissionDtos;

namespace CourseHub.Core.Services.Domain.AssignmentServices.Contracts;

public interface ISubmissionService
{
    Task<ServiceResult<SubmissionModel>> GetAsync(Guid id);
    Task<ServiceResult<List<SubmissionMinModel>>> GetByAssignmentId(Guid assignmentId, Guid clientId);

    Task<ServiceResult<Guid>> CreateAsync(CreateSubmissionDto dto, Guid client);
}