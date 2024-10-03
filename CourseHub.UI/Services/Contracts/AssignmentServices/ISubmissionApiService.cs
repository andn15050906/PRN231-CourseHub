using CourseHub.Core.Models.Assignment.SubmissionModels;
using CourseHub.Core.RequestDtos.Assignment.SubmissionDtos;

namespace CourseHub.UI.Services.Contracts.AssignmentServices;

public interface ISubmissionApiService
{
    Task<SubmissionModel?> GetAsync(Guid id, HttpContext context);
    Task<List<SubmissionMinModel>> GetByAssignmentAsync(Guid assignmentId, HttpContext context);

    Task<HttpResponseMessage> CreateAsync(CreateSubmissionDto dto, HttpContext context);
}
