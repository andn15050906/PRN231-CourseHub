using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Interfaces.Repositories.UserRepos;
using CourseHub.Infrastructure.AccessContext;
using CourseHub.Infrastructure.Repositories.CourseRepos;
using CourseHub.Infrastructure.Repositories.UserRepos;

namespace CourseHub.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;

    public UnitOfWork(Context context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }






    private UserRepository? _userRepo;
    public IUserRepository UserRepo { get => _userRepo ??= new UserRepository(_context); }



    private CategoryRepository? _categoryRepo;
    public ICategoryRepository CategoryRepo { get => _categoryRepo ??= new CategoryRepository(_context); }

    private CourseRepository? _courseRepo;
    public ICourseRepository CourseRepo { get => _courseRepo ??= new CourseRepository(_context); }
}
