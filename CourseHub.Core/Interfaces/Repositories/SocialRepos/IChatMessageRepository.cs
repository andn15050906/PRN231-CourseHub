using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Social;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.SocialRepos;

public interface IChatMessageRepository : IRepository<ChatMessage>
{
    IPagingQuery<ChatMessage, ChatMessageModel> GetPagingQuery(Expression<Func<ChatMessage, bool>>? whereExpression, short pageIndex, byte pageSize);
}