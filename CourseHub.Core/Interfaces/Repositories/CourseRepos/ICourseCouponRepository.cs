using CourseHub.Core.Interfaces.Repositories.Shared;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface ICourseCouponRepository : IRepository<CourseCoupon>
{
    Task<List<CourseCoupon>> GetByCourseAsync(Guid courseId);
}
