using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

internal class LectureRepository : BaseRepository<Lecture>, ILectureRepository
{
    public LectureRepository(DbContext context) : base(context)
    {
    }

    public Task<List<Lecture>> GetAllByCourseAsync(Guid course)
    {
        throw new NotImplementedException();
    }
}
