using CourseHub.Core.Helpers.Validation;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.Core.RequestDtos.Course.CourseReviewDtos;

public class CreateCourseReviewDto
{
    public Guid CourseId { get; set; }

    [Required]
    [RatingValidation]
    public byte Rating { get; set; }

    [StringLength(500)]
    public string Content { get; set; }
}
