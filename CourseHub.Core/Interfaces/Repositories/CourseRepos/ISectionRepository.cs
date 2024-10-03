using CourseHub.Core.Interfaces.Repositories.Shared;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface ISectionRepository : IRepository<Section>
{
    Task<Section?> GetWithCourse(Guid id);
    void RemoveRangeById(Guid courseId, IEnumerable<Guid> removed);
}
