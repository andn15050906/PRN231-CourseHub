using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;

namespace CourseHub.Core.Services.Domain.CourseServices.Contracts;

public interface IInstructorService
{
    Task<ServiceResult<PagedResult<InstructorModel>>> GetAsync(QueryInstructorDto dto);
    Task<ServiceResult<InstructorModel>> GetByUserIdAsync(Guid userId);

    //Task<ServiceResult<Guid>> CreateAsync(CreateInstructorDto dto, Guid creatorId);
    Task<ServiceResult> UpdateAsync(UpdateInstructorDto dto, Guid client);
}
