using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;

namespace CourseHub.Core.Services.Domain.AssignmentServices.Contracts;

public interface IAssignmentService
{
    Task<ServiceResult<AssignmentModel>> GetAsync(Guid id);
    Task<ServiceResult<AssignmentMinModel>> GetMinAsync(Guid id);
    Task<ServiceResult<List<AssignmentMinModel>>> GetBySectionsAsync(IEnumerable<Guid> sections);
    Task<ServiceResult<List<AssignmentMinModel>>> GetByCourseAsync(Guid courseId);

    Task<ServiceResult<Guid>> CreateAsync(CreateAssignmentDto dto, Guid client);
    Task<ServiceResult> UpdateAsync(UpdateAssignmentDto dto, Guid client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid client);
}
