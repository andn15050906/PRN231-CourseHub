using CourseHub.Core.Interfaces.Repositories.Shared;

namespace CourseHub.Core.Interfaces.Repositories.AssignmentRepos;

public interface IMcqChoiceRepository : IRepository<McqChoice>
{
    Task<List<McqChoice>> GetMultiple(IEnumerable<Guid> ids);
}
