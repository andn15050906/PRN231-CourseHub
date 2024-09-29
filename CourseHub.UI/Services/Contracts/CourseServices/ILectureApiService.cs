using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Models.Course.LectureModels;
using CourseHub.Core.RequestDtos.Course.LectureDtos;

namespace CourseHub.UI.Services.Contracts.CourseServices;

public interface ILectureApiService
{
    Task<Lecture?> GetAsync(Guid id, HttpContext context);
    Task<HttpResponseMessage> Create(CreateLectureDto dto, HttpContext context);
    Task<HttpResponseMessage> Update(UpdateLectureDto dto, HttpContext context);
    Task<HttpResponseMessage> Delete(Guid id, HttpContext context);
}
