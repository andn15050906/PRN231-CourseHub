using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Core.Interfaces.Repositories.CommonRepos;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using CourseHub.Core.Services.Mappers.CommonMappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.CommonRepos;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(DbContext context) : base(context)
    {
    }

    public IPagingQuery<Comment, CommentModel> GetPagingQuery(Expression<Func<Comment, bool>>? whereExpression, short pageIndex, byte pageSize)
    {
        return GetPagingQuery<CommentModel>(
            CommentMapperProfile.ModelConfig, whereExpression, pageIndex, pageSize,
            asSplitQuery: true, _ => _.Medias, _ => _.Reactions);
    }
}
