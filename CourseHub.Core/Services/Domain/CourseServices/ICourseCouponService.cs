using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.RequestDtos.Course.CourseCouponDtos;

namespace CourseHub.Core.Services.Domain.CourseServices;

public interface ICourseCouponService
{
    Task<ServiceResult<List<CourseCoupon>>> GetAsync(Guid courseId);

    Task<ServiceResult<Guid>> CreateAsync(CreateCourseCouponDto dto, Guid? client);
    Task<ServiceResult> UpdateAsync(UpdateCourseCouponDto dto, Guid? client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid? client);
}
