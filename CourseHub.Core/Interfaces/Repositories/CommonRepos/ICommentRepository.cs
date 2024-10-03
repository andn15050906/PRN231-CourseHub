using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.CommonRepos;

public interface ICommentRepository : IRepository<Comment>
{
    IPagingQuery<Comment, CommentModel> GetPagingQuery(Expression<Func<Comment, bool>>? whereExpression, short pageIndex, byte pageSize);
}
