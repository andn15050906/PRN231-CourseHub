using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.UserRepos;

public interface IUserRepository : IRepository<User>
{
    Task<UserFullModel?> GetFullAsync(Guid id);
    Task<UserModel?> GetAsync(Guid id);
    IPagingQuery<User, UserModel> GetPagingQuery(Expression<Func<User, bool>>? whereExpression, short pageIndex, byte pageSize);
    Task<List<UserOverviewModel>> GetOverviewAsync(List<Guid> ids);
    Task<Guid> GetIdByProviderKey(string key);

    Task<bool> UserNameExisted(string userName);
    Task<bool> EmailExisted(string email);
    Task<User?> FindByUserName(string userName);
    Task<User?> FindByEmail(string email);
}
