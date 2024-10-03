using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Models.Course.CourseModels;

public class CourseOverviewModel
{
    public Guid Id { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string MetaTitle { get; set; }
    public string ThumbUrl { get; set; }
    //public string Intro { get; set; }
    public CourseStatus Status { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public DateTime DiscountExpiry { get; set; }
    public CourseLevel Level { get; set; }
    //public string Outcomes { get; set; }
    public byte LectureCount { get; set; }
    public int LearnerCount { get; set; }
    public int RatingCount { get; set; }
    public long TotalRating { get; set; }
    public int BookmarkCount { get; set; }

    public Guid LeafCategoryId { get; set; }

    public UserMinModel Creator { get; set; }
}
