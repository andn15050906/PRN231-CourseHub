using CourseHub.Core.Entities.CommonDomain;
using CourseHub.Core.Interfaces.Repositories.CommonRepos;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.NotificationModels;
using CourseHub.Core.Services.Mappers.CommonMappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.CommonRepos;

internal class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    public NotificationRepository(DbContext context) : base(context)
    {
    }

    public IPagingQuery<Notification, NotificationModel> GetPagingQuery(Expression<Func<Notification, bool>>? whereExpression, short pageIndex, byte pageSize)
    {
        return GetPagingQuery<NotificationModel>(NotificationMapperProfile.ModelConfig, whereExpression, pageIndex, pageSize);
    }

    public async Task Insert(IEnumerable<Notification> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }
}
