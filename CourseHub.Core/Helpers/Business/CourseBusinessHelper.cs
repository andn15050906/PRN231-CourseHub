namespace CourseHub.Core.Helpers.Business;

public static class CourseBusinessHelper
{
    public static bool IsOnDiscount(double discount, DateTime discountExpiry)
    {
        return discount > 0 && discountExpiry > DateTime.UtcNow;
    }

    /// <summary>
    /// int
    /// </summary>
    public static int GetPostDiscount(double price, double discount, DateTime discountExpiry)
    {
        return IsOnDiscount(discount, discountExpiry)
            ? (int)(price * (1 - discount))
            : (int)price;
    }
}
