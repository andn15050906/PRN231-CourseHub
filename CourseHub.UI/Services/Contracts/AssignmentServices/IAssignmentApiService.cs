using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;

namespace CourseHub.UI.Services.Contracts.AssignmentServices;

public interface IAssignmentApiService
{
    Task<AssignmentModel?> GetAsync(Guid id, HttpContext context);
    Task<AssignmentMinModel?> GetMinAsync(Guid id);
    Task<List<AssignmentMinModel>> GetBySectionsAsync(IEnumerable<Guid> sections);
    Task<List<AssignmentMinModel>?> GetByCourseAsync(Guid courseId);

    Task<HttpResponseMessage> CreateAsync(CreateAssignmentDto dto, HttpContext context);
    Task<HttpResponseMessage> UpdateAsync(UpdateAssignmentDto dto, HttpContext context);
    Task<HttpResponseMessage> DeleteAsync(Guid id, HttpContext context);
}
