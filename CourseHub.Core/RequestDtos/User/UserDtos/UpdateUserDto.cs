using CourseHub.Core.Helpers.Messaging.Messages;
using CourseHub.Core.Helpers.Validation;
using CourseHub.Core.RequestDtos.Shared;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.Core.RequestDtos.User.UserDtos;

public class UpdateUserDto
{
    public string? FullName { get; set; }

    public CreateMediaDto? Avatar { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(1000, ErrorMessage = UserDomainMessages.INVALID_BIO_LENGTH)]
    public string? Bio { get; set; }

    /*[PhoneValidation]
    public string? Phone { get; set; }*/



    [PasswordValidation]
    public string? CurrentPassword { get; set; }
    [PasswordValidation]
    public string? NewPassword { get; set; }
}
