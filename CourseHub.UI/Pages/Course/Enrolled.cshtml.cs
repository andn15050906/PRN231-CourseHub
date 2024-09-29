using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Course;

public class EnrolledModel : PageModel
{
    public List<EnrollmentModel> Enrollments { get; set; }

    public async Task<IActionResult> OnGet([FromServices] ICourseApiService courseApiService)
    {
        var client = await HttpContext.GetClientData();
        if (client is null)
            return Redirect(Global.PAGE_SIGNIN);

        Enrollments = await courseApiService.GetEnrollmentsAsync(HttpContext);
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}
