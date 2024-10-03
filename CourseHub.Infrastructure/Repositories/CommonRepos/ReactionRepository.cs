using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Core.Interfaces.Repositories.CommonRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.CommonRepos;

public class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
{
    public ReactionRepository(DbContext context) : base(context)
    {
    }
}
