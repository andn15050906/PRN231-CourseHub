using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Core.Interfaces.Repositories.AssignmentRepos;
using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.Services.Mappers.AssignmentMappers;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.AssignmentRepos;

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(DbContext context) : base(context)
    {
    }

    public async Task<AssignmentModel?> GetAsync(Guid id)
    {
        return await DbSet
            .Include(_ => _.Questions).ThenInclude(_ => _.Choices)
            .ProjectTo<AssignmentModel>(AssignmentMapperProfile.ModelConfig)
            .FirstOrDefaultAsync(_ => _.Id == id);
    }

    public async Task<AssignmentMinModel?> GetMinAsync(Guid id)
    {
        return await DbSet
            .ProjectTo<AssignmentMinModel>(AssignmentMapperProfile.MinModelConfig)
            .FirstOrDefaultAsync(_ => _.Id == id);
    }

    public async Task<List<Guid>> GetIdsBySectionsAsync(IEnumerable<Guid> sectionIds)
    {
        return await DbSet
            .Where(_ => sectionIds.Contains(_.SectionId))
            .Select(_ => _.Id)
            .ToListAsync();
    }

    public async Task<List<AssignmentMinModel>> GetBySectionsAsync(IEnumerable<Guid> sectionIds)
    {
        return await DbSet
            .Where(_ => sectionIds.Contains(_.SectionId))
            .ProjectTo<AssignmentMinModel>(AssignmentMapperProfile.MinModelConfig)
            .ToListAsync();
    }
}
