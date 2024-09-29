using CourseHub.Core.Helpers.Messaging;

namespace CourseHub.Core.Services.Domain.CourseServices;

public interface IEnrollmentService
{
    /// <summary>
    /// Create the Enrollment object
    /// </summary>
    Task<ServiceResult> Enroll();

    /// <summary>
    /// Update the Enrollment
    /// </summary>
    Task<ServiceResult> Unenroll();
}
