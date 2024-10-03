using CourseHub.Core.Interfaces.Repositories.Shared;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface ILectureRepository : IRepository<Lecture>
{
    Task<List<Lecture>> GetAllByCourseAsync(Guid course);
}
