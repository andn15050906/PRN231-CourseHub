using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Models.Course.EnrollmentModels;

namespace CourseHub.Core.Services.Domain.CourseServices.Contracts;

public interface IEnrollmentService
{
    Task<ServiceResult<bool>> IsEnrolled(Guid courseId, Guid client);

    Task<ServiceResult<List<EnrollmentModel>>> Get(Guid client);
    Task<ServiceResult<EnrollmentFullModel>> GetFull(Guid courseId, Guid client);

    /// <summary>
    /// Not commited operation
    /// </summary>
    Task<ServiceResult> Enroll(Guid courseId, Guid client, Guid billId);

    /// <summary>
    /// Update the Enrollment
    /// </summary>
    Task<ServiceResult> Unenroll(Guid courseId, Guid client);

    Task ForceCommitAsync();
}
