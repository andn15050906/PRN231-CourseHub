using CourseHub.Core.Entities.SocialDomain;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Interfaces.Repositories.SocialRepos;
using CourseHub.Core.Models.Social;
using CourseHub.Core.Services.Mappers.ConversationMappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.SocialRepos;

public class ChatMessageRepository : BaseRepository<ChatMessage>, IChatMessageRepository
{
    public ChatMessageRepository(DbContext context) : base(context)
    {
    }

    public IPagingQuery<ChatMessage, ChatMessageModel> GetPagingQuery(Expression<Func<ChatMessage, bool>>? whereExpression, short pageIndex, byte pageSize)
    {
        return GetPagingQuery<ChatMessageModel>(
            ChatMessageMapperProfile.ModelConfig, whereExpression, pageIndex, pageSize,
            includeExpressions: _ => _.Reactions);
    }
}
