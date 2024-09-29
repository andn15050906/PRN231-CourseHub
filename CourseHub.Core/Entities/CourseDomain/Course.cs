using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Text;

namespace CourseHub.Core.Entities.CourseDomain;

#pragma warning disable CS8618

public class Course : AuditedEntity
{
    // Attributes
    public string Title { get; private set; }
    public string MetaTitle { get; private set; }
    public string ThumbUrl { get; set; }
    public string Intro { get; set; }
    public string Description { get; set; }
    public CourseStatus Status { get; set; }
    public double Price { get; set; }
    public double Discount { get; private set; }
    public DateTime DiscountExpiry { get; private set; }
    public CourseLevel Level { get; set; }
    public string Outcomes { get; private set; }
    public string Requirements { get; private set; }
    public byte LectureCount { get; private set; }
    public int LearnerCount { get; set; }
    public int RatingCount { get; private set; }
    public long TotalRating { get; private set; }
    public int BookmarkCount { get; set; }

    // FKs
    public Guid LeafCategoryId { get; set; }

    // Navigations
    public User? Creator { get; set; }
    public Category? LeafCategory { get; set; }
    public ICollection<Section> Sections { get; private set; }
    public ICollection<CourseMeta> Metas { get; private set; }
    public ICollection<CourseReview> Reviews { get; private set; }
    public ICollection<CourseCoupon> Coupons { get; private set; }

#pragma warning restore CS8618



    public void SetTitle(string title)
    {
        Title = title;
        MetaTitle = TextHelper.Normalize(title);
    }

    public void SetDiscount(double discount, DateTime expiry)
    {
        if (discount < 0 || discount > 1)
            throw new Exception(CourseDomainMessages.INVALID_DISCOUNT);
        if (expiry < DateTime.UtcNow)
            throw new Exception(CourseDomainMessages.INVALID_DISCOUNT_EXPIRY);
        Discount = discount;
        DiscountExpiry = expiry;
    }

    public void AddSections()
    {

    }

    public void AddCoupons()
    {

    }
}