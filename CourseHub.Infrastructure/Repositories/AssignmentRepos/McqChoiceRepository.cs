using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Core.Interfaces.Repositories.AssignmentRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.AssignmentRepos;

public class McqChoiceRepository : BaseRepository<McqChoice>, IMcqChoiceRepository
{
    public McqChoiceRepository(DbContext context) : base(context)
    {
    }

    public async Task<List<McqChoice>> GetMultiple(IEnumerable<Guid> ids)
    {
        return await DbSet.Where(_ => ids.Contains(_.Id)).ToListAsync();
    }
}
