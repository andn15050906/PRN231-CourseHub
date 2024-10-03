using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CommonServices;
using CourseHub.UI.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Instructor;

public class RequestModel : PageModel
{
    [BindProperty]
    public CreateInstructorDto Dto { get; set; }

    [BindProperty]
    public IFormFile? File { get; set; }



    public async Task<IActionResult> OnGet()
    {
        var Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        return Page();
    }

    public async Task OnPost([FromServices] INotificationApiService notificationApiService)
    {
        if (Dto.Intro.Length <= 50)
        {
            ModelState.AddModelError(string.Empty, "Intro must be more than 50 characters");
			return;
		}
		if (Dto.Experience.Length <= 50)
		{
			ModelState.AddModelError(string.Empty, "Experience must be more than 50 characters");
			return;
		}

		if (!ModelState.IsValid)
            return;



        var response = await notificationApiService.RequestInstructor(Dto, HttpContext);

        TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;
        if (!response.IsSuccessStatusCode)
        {
            TempData[Global.ALERT_MESSAGE] = "Cannot request to become Instructor!";
            return;
        }
        TempData[Global.ALERT_MESSAGE] = "Your request has been sent. Please wait for approval!";
    }
}
