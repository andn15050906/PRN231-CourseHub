using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(DbContext context) : base(context)
    {
    }

    public async Task<Section?> GetWithCourse(Guid id)
    {
        return await DbSet
            .Include(_ => _.Course).ThenInclude(_ => _.Creator)
            .FirstOrDefaultAsync(_ => _.Id == id);
    }

    public void RemoveRangeById(Guid courseId, IEnumerable<Guid> removed)
    {
        DbSet.RemoveRange(DbSet.Where(_ => removed.Contains(_.Id) && _.CourseId == courseId));
    }
}
