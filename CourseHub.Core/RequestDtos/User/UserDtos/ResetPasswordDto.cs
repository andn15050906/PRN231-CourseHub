using CourseHub.Core.Helpers.Validation;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.Core.RequestDtos.User.UserDtos;

public class ResetPasswordDto
{
    public string Email { get; set; }

    public string Token { get; set; }

    [Required]
    [PasswordValidation]
    public string NewPassword { get; set; }
}
