using AutoMapper;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;

namespace CourseHub.Core.Services.Domain.CourseServices;

public class EnrollmentService : DomainService, IEnrollmentService
{
    public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger logger) : base(unitOfWork, mapper, logger)
    {
    }

    public async Task<ServiceResult<bool>> IsEnrolled(Guid courseId, Guid client)
    {
        bool result = await _uow.EnrollmentRepo.IsEnrolled(client, courseId);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<EnrollmentModel>>> Get(Guid client)
    {
        var result = await _uow.EnrollmentRepo.Get(client);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<EnrollmentFullModel>> GetFull(Guid courseId, Guid client)
    {
        var result = await _uow.EnrollmentRepo.Get(courseId, client);
        return ToQueryResult(result);
    }

    public async Task<ServiceResult<List<EnrollmentModel>>> GetLearners(Guid courseId, Guid client)
    {
        var result = await _uow.EnrollmentRepo.GetByCourse(courseId);
        return ToQueryResult(result);
    }



    public async Task<ServiceResult> Enroll(Guid courseId, Guid client, Guid billId)
    {
        var entity = new Enrollment(courseId, client, billId);
        await _uow.EnrollmentRepo.Insert(entity);
        return Ok();
    }

    public async Task<ServiceResult> Unenroll(Guid courseId, Guid client)
    {
        var entity = await _uow.EnrollmentRepo.Find(courseId, client);
        if (entity is null)
            return BadRequest();

        _uow.EnrollmentRepo.Delete(entity);
        return Ok();
    }



    public async Task ForceCommitAsync()
    {
        await _uow.CommitAsync();
    }
}
