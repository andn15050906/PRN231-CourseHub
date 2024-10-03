using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface ICourseRepository : IRepository<Course>
{
    Task<CourseModel?> GetAsync(Guid id);
    IPagingQuery<Course, CourseOverviewModel> GetPagingQuery(Expression<Func<Course, bool>>? whereExpression, short pageIndex, byte pageSize, params Expression<Func<Course, object?>>[]? includeExpressions);
    IPagingQuery<Course, CourseMinModel> GetPagingQuery(Expression<Func<Course, bool>>? whereExpression, short pageIndex, byte pageSize);
    Task<List<CourseOverviewModel>> GetMultipleAsync(IEnumerable<Guid> ids);
    Task<List<CourseOverviewModel>> GetSimilar(Guid id);
    Task<CourseMinModel?> GetMinAsync(Guid id);

    Task<CourseSectionsModel?> GetCourseSections(Guid sectionId);
    void LoadSections(Course course);

    //...
    Task<List<Course>> GetAllAsync();
}
