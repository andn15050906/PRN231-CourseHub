using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Models.Course.LectureModels;
using CourseHub.Core.RequestDtos.Course.LectureDtos;

namespace CourseHub.Core.Services.Domain.CourseServices.Contracts;

public interface ILectureService
{
    Task<ServiceResult<Lecture>> GetAsync(Guid id);

    Task<ServiceResult<Guid>> CreateAsync(CreateLectureDto dto, Guid client);
    Task<ServiceResult> UpdateAsync(UpdateLectureDto dto, Guid client);
    Task<ServiceResult> DeleteAsync(Guid id, Guid client);
}
