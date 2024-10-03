using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.CourseRepos;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories.CourseRepos;

public class CourseCouponRepository : BaseRepository<CourseCoupon>, ICourseCouponRepository
{
    public CourseCouponRepository(DbContext context) : base(context)
    {
    }

    public Task<List<CourseCoupon>> GetByCourseAsync(Guid courseId)
    {
        throw new NotImplementedException();
    }
}
