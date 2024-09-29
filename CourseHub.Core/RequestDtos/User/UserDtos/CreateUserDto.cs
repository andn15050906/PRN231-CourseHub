using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Validation;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.Core.RequestDtos.User.UserDtos;

public class CreateUserDto
{
    [EmailAddress(ErrorMessage = UserDomainMessages.INVALID_EMAIL)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20, MinimumLength = 6, ErrorMessage = UserDomainMessages.INVALID_USERNAME)]
    public string UserName { get; set; } = string.Empty;

    [PasswordValidation]
    public string Password { get; set; } = string.Empty;

    public Role Role { get; set; }
}
