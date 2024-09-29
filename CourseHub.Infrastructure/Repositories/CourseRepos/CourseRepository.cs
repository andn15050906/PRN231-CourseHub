using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Services.Mappers.CourseMappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(DbContext context) : base(context)
    {
    }

    public async Task<CourseModel?> GetAsync(Guid id)
    {
        return await DbSet
            .Where(_ => _.Id == id)
            .ProjectTo<CourseModel>(CourseMapperProfile.ModelConfig)
            .FirstOrDefaultAsync();
    }

    public IPagingQuery<Course, CourseOverviewModel> GetPagingQuery(Expression<Func<Course, bool>>? whereExpression, short pageIndex, byte pageSize)
    {
        return GetPagingQuery<CourseOverviewModel>(CourseMapperProfile.OverviewModelConfig, whereExpression, pageIndex, pageSize);
    }

    public Task<List<CourseOverviewModel>> GetSimilar(Guid id)
    {
        throw new NotImplementedException();
    }
}
