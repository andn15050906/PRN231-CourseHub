using AutoMapper;
using CourseHub.Core.Entities.Contracts;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Infrastructure.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : DomainObject
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(DbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }



    public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        => await DbSet.AsNoTracking().AnyAsync(predicate);

    public async Task<T?> Find(params object[] keys)
        => await DbSet.FindAsync(keys);

    public virtual async Task Insert(T entity)
        => await DbSet.AddAsync(entity);

    public void Delete(T entity)
        => DbSet.Remove(entity);

    /// <summary>
    /// Concrete repositories provide mapping configuration & included properties
    /// Might use this method or call PagingQuery's constructor within Concrete repositories
    /// </summary>
    protected PagingQuery<T, TDto> GetPagingQuery<TDto>(
        IConfigurationProvider mappingConfig,
        Expression<Func<T, bool>>? whereExpression,
        short pageIndex, byte pageSize,
        bool asSplitQuery = false, params Expression<Func<T, object?>>[] includeExpressions)
    {
        return new(DbSet, mappingConfig, whereExpression, pageIndex, pageSize, asSplitQuery, includeExpressions);
    }
}
