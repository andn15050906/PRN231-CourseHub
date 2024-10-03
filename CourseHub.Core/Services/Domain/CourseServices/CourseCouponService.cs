using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.RequestDtos.Course.CourseCouponDtos;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;

namespace CourseHub.Core.Services.Domain.CourseServices;

public class CourseCouponService : DomainService, ICourseCouponService
{
    public CourseCouponService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }

    public Task<ServiceResult<List<CourseCoupon>>> GetAsync(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<Guid>> CreateAsync(CreateCourseCouponDto dto, Guid? client)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> DeleteAsync(Guid id, Guid? client)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> UpdateAsync(UpdateCourseCouponDto dto, Guid? client)
    {
        throw new NotImplementedException();
    }
}
