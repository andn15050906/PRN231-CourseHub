using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

internal class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DbContext context) : base(context)
    {
    }

    public Task<List<Category>> GetAllAsync() => DbSet.ToListAsync();

    public async Task ExecuteDeleteAsync(Guid id)
    {
        await DbSet.Where(_ => _.Id == id).ExecuteDeleteAsync();
    }
}
