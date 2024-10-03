using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Validation;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.Core.RequestDtos.User.UserDtos;

public class SignInDto
{
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = UserDomainMessages.INVALID_EMAIL)]
    public string? Email { get; set; }

    [Required]
    [PasswordValidation]
    public string Password { get; set; } = null!;
}
