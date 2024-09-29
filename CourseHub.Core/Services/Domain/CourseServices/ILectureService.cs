using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.RequestDtos.Course.LectureDtos;

namespace CourseHub.Core.Services.Domain.CourseServices;

public interface ILectureService
{
    Task<ServiceResult<Lecture>> GetAsync(Guid lecture);

    Task<ServiceResult<Guid>> CreateAsync(CreateLectureDto dto, Guid? client);
    Task<ServiceResult> UpdateAsync(UpdateLectureDto dto, Guid? client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid? client);
}
