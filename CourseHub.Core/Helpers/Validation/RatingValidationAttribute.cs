using CourseHub.Core.Helpers.Messaging.Messages;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CourseHub.Core.Helpers.Validation;

/// <summary>
/// [Range(1, 5)]
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
internal class RatingValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = CourseDomainMessages.INVALID_RATING;

        // RequiredAttribute should be used to assert a value is not empty.
        if (value is null)
            return true;

        if (!byte.TryParse(value.ToString(), out var result))
            return false;
        return result > 0 && result < 6;
    }
}
