using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.User;

public class IndexModel : PageModel
{
    public UserModel? UserModel { get; set; }
    public InstructorModel? InstructorModel { get; set; }



    public async Task<IActionResult> OnGet(
        [FromQuery] Guid id,
        [FromServices] IUserApiService userApiService, [FromServices] IInstructorApiService instructorApiService)
    {
        UserModel = await userApiService.GetAsync(id);
        if (UserModel is null)
            return Redirect(Global.PAGE_404);

        if (UserModel.Role == Core.Entities.UserDomain.Enums.Role.Instructor)
            InstructorModel = await instructorApiService.GetByUserId(UserModel.Id);

        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}
