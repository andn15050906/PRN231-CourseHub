using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.Models.Course.CourseReviewModels;
using CourseHub.Core.Models.Course.SectionModels;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Models.Course.CourseModels;

public class CourseModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string MetaTitle { get; set; }
    public string ThumbUrl { get; set; }
    public string Intro { get; set; }
    public CourseStatus Status { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public CourseLevel Level { get; set; }
    public string Outcomes { get; set; }
    public byte LectureCount { get; set; }
    public int LearnerCount { get; set; }
    public int RatingCount { get; set; }
    public long TotalRating { get; set; }
    public int BookmarkCount { get; set; }

    public Guid LeafCategoryId { get; set; }

    public string Description { get; set; }
    public DateTime DiscountExpiry { get; set; }
    public string Requirements { get; set; }

    public Guid InstructorId { get; set; }

    public UserModel Creator { get; set; }
    public List<SectionModel> Sections { get; set; }
    public List<CourseMeta> Metas { get; set; }
    public List<CourseReviewModel> Reviews { get; set; }
    //public List<CourseCoupon> Coupons { get; set; }
}
