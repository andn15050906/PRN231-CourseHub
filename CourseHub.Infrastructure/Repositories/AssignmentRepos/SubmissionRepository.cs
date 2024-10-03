using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Core.Interfaces.Repositories.AssignmentRepos;
using CourseHub.Core.Models.Assignment.SubmissionModels;
using CourseHub.Core.Services.Mappers.AssignmentMappers;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.AssignmentRepos;

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(DbContext context) : base(context)
    {
    }

    public async Task<SubmissionModel?> Get(Guid id)
    {
        return await DbSet
            .Include(_ => _.Answers)
            .AsNoTracking()
            .ProjectTo<SubmissionModel>(SubmissionMapperProfile.ModelConfig)
            .FirstOrDefaultAsync(_ => _.Id == id);
    }

    public async Task<List<SubmissionMinModel>> GetByAssignmentId(Guid assignmentId, Guid creatorId)
    {
        return await DbSet
            .Where(_ => _.AssignmentId == assignmentId && _.CreatorId == creatorId)
            .OrderBy(_ => _.CreationTime)
            .ProjectTo<SubmissionMinModel>(SubmissionMapperProfile.MinModelConfig)
            .ToListAsync();
    }
}
