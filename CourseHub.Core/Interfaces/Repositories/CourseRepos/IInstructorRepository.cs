using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.InstructorModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface IInstructorRepository : IRepository<Instructor>
{
    Task<InstructorModel?> GetAsync(Guid id);
    IPagingQuery<Instructor, InstructorModel> GetPagingQuery(Expression<Func<User, bool>>? whereExpression, short pageIndex, byte pageSize);
}
