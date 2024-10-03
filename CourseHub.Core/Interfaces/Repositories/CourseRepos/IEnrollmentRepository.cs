using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.EnrollmentModels;

namespace CourseHub.Core.Interfaces.Repositories.CourseRepos;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> IsEnrolled(Guid userId, Guid courseId);
    Task<List<EnrollmentModel>> Get(Guid creatorId);
    Task<List<EnrollmentModel>> GetByCourse(Guid courseId);
    Task<EnrollmentFullModel?> Get(Guid courseId, Guid creatorId);
}