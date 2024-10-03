using System.Linq.Expressions;

namespace CourseHub.Core.Interfaces.Repositories.Shared;

public interface IPagingQuery<T, TDto> where T : DomainObject
{
    Task<PagedResult<TDto>> ExecuteWithOrderBy<TOrderBy>(Expression<Func<T, TOrderBy>> orderByExpression, bool ascending = true, bool isAnsiWarningTransaction = false, bool asNoTracking = false);
}
