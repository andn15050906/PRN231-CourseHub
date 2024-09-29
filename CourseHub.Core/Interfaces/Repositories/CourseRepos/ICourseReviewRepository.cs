using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseReviewModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface ICourseReviewRepository : IRepository<CourseReview>
{
    IPagingQuery<CourseReview, CourseReviewModel> GetPagingQuery(Expression<Func<User, bool>>? whereExpression, short pageIndex, byte pageSize);
}
