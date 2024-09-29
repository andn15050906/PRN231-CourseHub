using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Core.Interfaces.Repositories.CommonRepos;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

internal class CommentRepository : ICommentRepository
{
    public IPagingQuery<Comment, CommentModel> GetPagingQuery(Expression<Func<Comment, bool>>? whereExpression, short pageIndex, byte pageSize)
    {
        throw new NotImplementedException();
    }
}
