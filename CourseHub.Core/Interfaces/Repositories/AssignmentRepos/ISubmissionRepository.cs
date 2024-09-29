using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Assignment.SubmissionModels;

namespace CourseHub.Core.Interfaces.Repositories.AssignmentRepos;

public interface ISubmissionRepository : IRepository<Submission>
{
    Task<SubmissionModel?> Get(Guid id);
    Task<List<SubmissionMinModel>> GetByAssignmentId(Guid assignmentId, Guid creatorId);
}
