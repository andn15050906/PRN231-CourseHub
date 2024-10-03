using CourseHub.Core.Helpers.Messaging.Messages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CourseHub.Core.Helpers.Validation;

/// <summary>
/// [RegularExpression(@"0\d{9,10}")]
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal class PhoneValidationAttribute : ValidationAttribute
{
    private const string PATTERN = @"0\d{9,10}";

    public override bool IsValid(object? value)
    {
        ErrorMessage = UserDomainMessages.INVALID_PHONE;

        // RequiredAttribute should be used to assert a value is not empty.
        if (value is null)
            return true;

        string sValue = value.ToString()!;
        return Regex.IsMatch(sValue, PATTERN);
    }
}
