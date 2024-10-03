using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.RequestDtos.Shared;

namespace CourseHub.Core.RequestDtos.Course.CourseDtos;

public class CreateCourseDto
{
    public string Title { get; set; }
    // MetaTitle: generated
    public CreateMediaDto Thumb { get; set; }
    public string Intro { get; set; }
    public string Description { get; set; }
    // Status: Ongoing
    public double Price { get; set; }
    // Discount
    // DiscountExpiry
    public CourseLevel Level { get; set; }
    public string Outcomes { get; set; }
    public string Requirements { get; set; }

    public Guid LeafCategoryId { get; set; }
    public List<string> SectionNames { get; set; }
    public List<CUCourseMetaDto>? Metas { get; set; }



    public class CUCourseMetaDto
    {
        public CourseMetaType Type { get; set; }
        public string Value { get; set; }
    }
}
