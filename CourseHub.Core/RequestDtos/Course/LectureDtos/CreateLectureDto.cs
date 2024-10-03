using CourseHub.Core.Entities.CourseDomain.Enums;
using Microsoft.AspNetCore.Http;

namespace CourseHub.Core.RequestDtos.Course.LectureDtos;

public class CreateLectureDto
{
    public Guid SectionId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool? IsPreviewable { get; set; }
    public List<CreateLectureMaterialDto> Materials { get; set; }

    public class CreateLectureMaterialDto
    {
        public LectureMaterialType Type { get; set; }
        public string? Url { get; set; }
        public IFormFile? File { get; set; }
    }
}