using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Entities.UserDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.Services.Mappers.CourseMappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

internal class InstructorRepository : BaseRepository<Instructor>, IInstructorRepository
{
    public InstructorRepository(DbContext context) : base(context)
    {
    }

    

    public async Task<Guid> GetIdByUserId(Guid userId)
    {
        return await DbSet
            .Where(_ => _.CreatorId == userId)
            .Take(1)
            .Select(_ => _.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<Instructor?> FindEntityByUserIdAsync(Guid userId)
    {
        return await DbSet.FirstOrDefaultAsync(_ => _.CreatorId == userId);
    }

    public async Task<InstructorModel?> GetByUserIdAsync(Guid userId)
    {
        return await DbSet
            .Where(_ => _.CreatorId == userId)
            .Take(1)
            .ProjectTo<InstructorModel>(InstructorMapperProfile.ModelConfig)
            .FirstOrDefaultAsync();
    }

    public async Task<InstructorModel?> GetAsync(Guid id)
    {
        return await DbSet
            .Where(_ => _.Id == id)
            .Take(1)
            .ProjectTo<InstructorModel>(InstructorMapperProfile.ModelConfig)
            .FirstOrDefaultAsync();
    }

    public IPagingQuery<Instructor, InstructorModel> GetPagingQuery(Expression<Func<Instructor, bool>>? whereExpression, short pageIndex, byte pageSize)
    {
        return GetPagingQuery<InstructorModel>(
            InstructorMapperProfile.ModelConfig, whereExpression, pageIndex, pageSize);
    }
}
