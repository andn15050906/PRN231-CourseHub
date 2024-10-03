namespace CourseHub.Core.Helpers.Messaging.Messages;

internal static class CourseDomainMessages
{
    // 400
    internal const string INVALID_PARENT = "Invalid parent";
    internal const string INVALID_USER = "Invalid user";
    internal const string INVALID_DISCOUNT = "Discount must be between 0 and 100%";
    internal const string INVALID_DISCOUNT_EXPIRY = "Invalid discount expiration date";
    internal const string INVALID_SECTION = "Invalid section";
    internal const string INVALID_LECTURE = "Invalid lecture";
    internal const string INVALID_RATING = "The rating must be between 1 and 5";
    internal const string INVALID_INSTRUCTOR = "Invalid instructor";
    internal const string INVALID_AMOUNT = "Invalid balance amount";

    // 500
    internal const string INTERNAL_BAD_MILESTONES = "Invalid milestones";
}