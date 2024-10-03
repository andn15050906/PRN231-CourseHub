using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.Core.Services.Mappers.CourseMappers;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

public class EnrollmentRepository : BaseRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(DbContext context) : base(context)
    {
    }



    public async Task<bool> IsEnrolled(Guid userId, Guid courseId)
    {
        return await DbSet.AnyAsync(_ => _.CreatorId == userId && _.CourseId == courseId);
    }

    public async Task<List<EnrollmentModel>> Get(Guid creatorId)
    {
        return await DbSet
            .Where(_ => _.CreatorId == creatorId)
            .Include(_ => _.Course)
            .Include(_ => _.Bill)
            .AsNoTracking()
            .ProjectTo<EnrollmentModel>(EnrollmentMapperProfile.ModelConfig)
            .ToListAsync();
    }

    public async Task<List<EnrollmentModel>> GetByCourse(Guid courseId)
    {
        return await DbSet
            .Where(_ => _.CourseId == courseId)
            .Include(_ => _.Course)
            .Include(_ => _.Bill)
            .AsNoTracking()
            .ProjectTo<EnrollmentModel>(EnrollmentMapperProfile.ModelConfig)
            .ToListAsync();
    }

    public async Task<EnrollmentFullModel?> Get(Guid courseId, Guid creatorId)
    {
        return await DbSet
            .Where(_ => _.CreatorId == creatorId && _.CourseId == courseId)
            .ProjectTo<EnrollmentFullModel>(EnrollmentMapperProfile.FullModelConfig)
            .FirstOrDefaultAsync();
    }
}
