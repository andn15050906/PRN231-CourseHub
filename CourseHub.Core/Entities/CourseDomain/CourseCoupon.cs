using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Text;

namespace CourseHub.Core.Entities.CourseDomain;

public class CourseCoupon : TimeAuditedEntity
{
    // Attributes
    public string Code { get; set; }
    public double Discount { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public bool IsRedeemed { get; set; }
    public bool IsGlobalGenerated { get; set; }

    // FKs
    public Guid CourseId { get; set; }

    // Navigations
    public Course? Course { get; set; }





    /// <summary>
    /// Generate a random Code, but tracked by CourseId and Id
    /// </summary>
    public void SetCode()
    {
        Code = TextHelper.GenerateAlphanumeric(6);
    }

    public void SetDiscount(double discount, DateTime expiry)
    {
        if (discount < 0 || discount > 1)
            throw new Exception(CourseDomainMessages.INVALID_DISCOUNT);
        if (expiry < DateTime.UtcNow)
            throw new Exception(CourseDomainMessages.INVALID_DISCOUNT_EXPIRY);
        Discount = discount;
        ExpiryDate = expiry;
    }
}
