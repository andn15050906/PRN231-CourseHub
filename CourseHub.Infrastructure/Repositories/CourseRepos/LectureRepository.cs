using AutoMapper.QueryableExtensions;
using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using CourseHub.Core.Models.Course.LectureModels;
using CourseHub.Core.Services.Mappers.CourseMappers;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

public class LectureRepository : BaseRepository<Lecture>, ILectureRepository
{
    public LectureRepository(DbContext context) : base(context)
    {
    }



    /*public async Task<LectureFullModel?> GetFullAsync(Guid id)
    {
        return await DbSet
            .Include(_ => _.Section)
            .ProjectTo<LectureFullModel>(LectureMapperProfile.FullModelConfig)
            .FirstOrDefaultAsync(_ => _.Id == id);
    }*/

    public Task<List<Lecture>> GetAllByCourseAsync(Guid course)
    {
        throw new NotImplementedException();
    }
}
