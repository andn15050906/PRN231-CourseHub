using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Models.Course.CourseModels;

public class CourseOverviewModel
{
    public string Title { get; private set; }
    public string MetaTitle { get; private set; }
    public string ThumbUrl { get; set; }
    public string Intro { get; set; }
    public CourseStatus Status { get; set; }
    public double Price { get; set; }
    public double Discount { get; private set; }
    public CourseLevel Level { get; set; }
    public string Outcomes { get; private set; }
    public byte LectureCount { get; private set; }
    public int LearnerCount { get; set; }
    public int RatingCount { get; private set; }
    public long TotalRating { get; private set; }
    public int BookmarkCount { get; set; }

    public Guid LeafCategoryId { get; set; }

    public UserMinModel Creator { get; set; }

    public DateTime LastModificationTime { get; protected set; }
}
