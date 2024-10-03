using static CourseHub.Core.RequestDtos.Course.LectureDtos.CreateLectureDto;

namespace CourseHub.Core.RequestDtos.Course.LectureDtos;

public class UpdateLectureDto
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public bool? IsPreviewable { get; set; }

    public List<CreateLectureMaterialDto>? AddedMaterials { get; set; }
    public List<string>? RemovedMaterials { get; set; }
}
