namespace CourseHub.Core.Helpers.Messaging.Messages;

internal static class UserDomainMessages
{
    // Mostly 400
    internal const string INVALID_EMAIL = "Invalid Email";
    internal const string INVALID_USERNAME = "UserName must be between 6 and 20 characters";
    internal const string INVALID_PASSWORD_6TO20CHARS = "Password must be between 6 and 20 characters";
    internal const string INVALID_PASSWORD_REGEX = "Password must contain Uppercase, Lowercase and Number";
    internal const string INVALID_PHONE = "Invalid phone number";
    internal const string INVALID_BIO_LENGTH = "Invalid Bio length";
    internal const string INVALID_NEWPASSWORD_MISSING = "New Password missing";
    internal const string INVALID_RESETPASSWORD_ATTEMPT = "Invalid reset password attempt";
    internal const string INVALID_EMAILPHONE_MISSING = "Must provide either email or phone";

    // 401
    internal const string UNAUTHORIZED_PASSWORD = "Password does not match";
    internal const string UNAUTHORIZED_SIGNIN = "Incorrect signin information";

    // 403
    internal const string FORBIDDEN_FAILED_EXCEED = "Access Failed Count exceed maximum";
    internal const string FORBIDDEN_NOT_APPROVED = "You are not approved";

    // 409
    internal const string CONFLICT_EMAIL = "This email has already been used";
}
