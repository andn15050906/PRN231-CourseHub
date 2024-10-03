using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.InstructorModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface IInstructorRepository : IRepository<Instructor>
{
    Task<Guid> GetIdByUserId(Guid userId);
    Task<Instructor?> FindEntityByUserIdAsync(Guid userId);

    Task<InstructorModel?> GetAsync(Guid id);
    Task<InstructorModel?> GetByUserIdAsync(Guid userId);

    IPagingQuery<Instructor, InstructorModel> GetPagingQuery(Expression<Func<Instructor, bool>>? whereExpression, short pageIndex, byte pageSize);
}
