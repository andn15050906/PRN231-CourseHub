namespace CourseHub.Core.Helpers.Messaging.Messages;

internal static class UserDomainMessages
{
    // Mostly 400
    internal const string INVALID_EMAIL = "400: Invalid Email";
    internal const string INVALID_USERNAME = "400: UserName must be between 6 and 20 characters";
    internal const string INVALID_PASSWORD_6TO20CHARS = "400: Password must be between 6 and 20 characters";
    internal const string INVALID_PASSWORD_REGEX = "400: Password must contain Uppercase, Lowercase and Number";
    internal const string INVALID_PHONE = "400: Invalid phone number";
    internal const string INVALID_BIO_LENGTH = "400: Invalid Bio length";
    internal const string INVALID_NEWPASSWORD_MISSING = "400: New Password missing";
    internal const string INVALID_RESETPASSWORD_ATTEMPT = "400: Invalid reset password attempt";
    internal const string INVALID_EMAILPHONE_MISSING = "400: Must provide either email or phone";

    // 401
    internal const string UNAUTHORIZED_PASSWORD = "401: Password does not match";
    internal const string UNAUTHORIZED_SIGNIN = "401: Incorrect signin information";

    // 403
    internal const string FORBIDDEN_FAILED_EXCEED = "403: You are forbidden to access";
    internal const string FORBIDDEN_NOT_APPROVED = "403: You are not approved";

    // 404
    internal const string NOT_FOUND = "User not found";

    // 409
    internal const string CONFLICT_EMAIL = "This email has already been used";
    internal const string CONFLICT_USERNAME = "This username has already been taken";
}
