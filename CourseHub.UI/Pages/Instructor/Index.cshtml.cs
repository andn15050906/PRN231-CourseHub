using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Instructor;

public class IndexModel : PageModel
{
    private readonly IInstructorApiService _instructorApiService;

    public UserFullModel? Client { get; set; }
    public InstructorModel? Instructor { get; set; }

    [BindProperty]
    public UpdateInstructorDto Dto { get; set; }



    public IndexModel(IInstructorApiService instructorApiService)
    {
        _instructorApiService = instructorApiService;
    }



    public async Task<IActionResult> OnGet()
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        Instructor = await _instructorApiService.GetByUserId(Client.Id);
        if (Instructor is null)
            return Redirect(Global.PAGE_403);

        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        //...
        if (Dto.Intro is null)
            Dto.Intro = string.Empty;
        if (Dto.Experience is null)
            Dto.Experience = string.Empty;

        var response = await _instructorApiService.Update(Dto, HttpContext);

        TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;
        TempData[Global.ALERT_MESSAGE] = response.IsSuccessStatusCode
            ? "Updated successfully."
            : $"Error updating!";

        return await OnGet();
    }
}
