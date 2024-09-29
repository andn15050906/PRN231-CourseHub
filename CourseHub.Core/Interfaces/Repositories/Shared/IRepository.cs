using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.Shared;

public interface IRepository<T> where T : DomainObject
{
    //protected abstract IEnumerable<T> FromSqlRaw(string query);

    Task<bool> Any(Expression<Func<T, bool>> predicate);
    Task<T?> Find(params object[] keys);

    Task Insert(T entity);
    void Delete(T entity);
}
