using AutoMapper;
using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.Contracts;
using CourseHub.Core.Interfaces.Repositories.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Helpers.Pagination;


/// <summary>
/// Not strict
/// 
/// includeExpressions should be references only
/// .ThenInclude(exp) not configured
/// .AsSplitQuery() placement should be benchmarked.
/// 
/// orderBy .ThenBy() not configured.
/// </summary>
public class PagingQuery<T, TDto> : IPagingQuery<T, TDto> where T : DomainObject
{
    private readonly IQueryable<T> _unorderedQuery;
    private IQueryable<T> _orderedQuery;

    private readonly short _pageIndex;
    private readonly byte _pageSize;
    private readonly IConfigurationProvider _mappingConfig;

    /// <summary>
    /// dtoType is for specifying the implied type.
    /// </summary>
    public PagingQuery(
        DbSet<T> dbSet, IConfigurationProvider mappingConfig,
        Expression<Func<T, bool>>? whereExpression,
        short pageIndex, byte pageSize,
        bool asSplitQuery = false, params Expression<Func<T, object?>>[] includeExpressions)
    {
        _unorderedQuery = dbSet;

        if (includeExpressions is not null)
        {
            if (asSplitQuery)
                _unorderedQuery = _unorderedQuery.AsSplitQuery();
            byte i;
            for (i = 0; i < includeExpressions.Length; i++)
                _unorderedQuery = _unorderedQuery.Include(includeExpressions[i]);
        }

        if (whereExpression is not null)
            _unorderedQuery = _unorderedQuery.Where(whereExpression);

        _orderedQuery = _unorderedQuery;

        _pageIndex = pageIndex;
        _pageSize = pageSize;
        _mappingConfig = mappingConfig;
    }

    public async Task<PagedResult<TDto>> ExecuteWithOrderBy<TOrderBy>(Expression<Func<T, TOrderBy>> orderByExpression, bool ascending = true)
    {
        int total = await _unorderedQuery.CountAsync();

        _orderedQuery = ascending ? _unorderedQuery.OrderBy(orderByExpression) : _unorderedQuery.OrderByDescending(orderByExpression);
        var items = await _orderedQuery
            .Skip(_pageIndex * _pageSize).Take(_pageSize)
            .ProjectTo<TDto>(_mappingConfig).ToListAsync();

        return new PagedResult<TDto>(total, _pageIndex, _pageSize, items);
    }
}