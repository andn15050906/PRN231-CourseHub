using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.NotificationModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.CommonRepos;

public interface INotificationRepository : IRepository<Notification>
{
    IPagingQuery<Notification, NotificationModel> GetPagingQuery(Expression<Func<Notification, bool>>? whereExpression, short pageIndex, byte pageSize);
    Task Insert(IEnumerable<Notification> entities);
}
