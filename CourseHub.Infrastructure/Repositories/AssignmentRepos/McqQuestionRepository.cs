using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Core.Interfaces.Repositories.AssignmentRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.AssignmentRepos;

public class McqQuestionRepository : BaseRepository<McqQuestion>, IMcqQuestionRepository
{
    public McqQuestionRepository(DbContext context) : base(context)
    {
    }
}
