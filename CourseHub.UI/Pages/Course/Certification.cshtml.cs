using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Course;

public class CertificationModel : PageModel
{
    private readonly ICourseApiService _courseApiService;

    public EnrollmentFullModel Enrollment { get; set; }
    public UserFullModel Client { get; set; }
    public CourseModel Course { get; set; }



    public CertificationModel(ICourseApiService courseApiService)
    {
        _courseApiService = courseApiService;
    }



    public async Task<IActionResult> OnGet([FromQuery] Guid courseId)
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        Enrollment = await _courseApiService.GetEnrollmentAsync(HttpContext, courseId);
        if (Enrollment is null)
            return Redirect(Global.PAGE_404);

        Course = await _courseApiService.GetAsync(courseId);
        if (Course is null)
            return Redirect(Global.PAGE_404);
        return Page();
    }
}
