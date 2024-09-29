using CourseHub.Core.Helpers.Validation;

namespace CourseHub.Core.RequestDtos.User.UserDtos;

public class ResetPasswordDto
{
    public string Email { get; set; }

    public string Token { get; set; }

    [PasswordValidation]
    public string NewPassword { get; set; }
}
