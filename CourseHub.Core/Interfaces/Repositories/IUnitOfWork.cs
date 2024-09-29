using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Interfaces.Repositories.UserRepos;

namespace CourseHub.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync();

    IUserRepository UserRepo { get; }

    ICategoryRepository CategoryRepo { get; }
    ICourseRepository CourseRepo { get; }
}
