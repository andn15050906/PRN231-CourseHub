using CourseHub.Core.Entities.SocialDomain;
using CourseHub.Core.Interfaces.Repositories.SocialRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.SocialRepos;

public class TagRepository : BaseRepository<Tag>, ITagRepository
{
    public TagRepository(DbContext context) : base(context)
    {
    }
}
