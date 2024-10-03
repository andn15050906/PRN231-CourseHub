using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.RequestDtos.Shared;
using static CourseHub.Core.RequestDtos.Course.CourseDtos.CreateCourseDto;

namespace CourseHub.Core.RequestDtos.Course.CourseDtos;

public class UpdateCourseDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public CreateMediaDto? Thumb { get; set; }
    public string? Intro { get; set; }
    public string? Description { get; set; }
    public CourseStatus? Status { get; set; }
    public double? Price { get; set; }

    public double? Discount { get; set; }
    public DateTime? DiscountExpiry { get; set; }

    public CourseLevel? Level { get; set; }
    public string? Outcomes { get; set; }
    public string? Requirements { get; set; }

    public Guid? LeafCategoryId { get; set; }

    public List<Guid>? RemovedSections { get; set; }
    public List<string>? AddedSections { get; set; }

    public List<CUCourseMetaDto>? RemovedMetas { get; set; }
    public List<CUCourseMetaDto>? AddedMetas { get; set; }
}
