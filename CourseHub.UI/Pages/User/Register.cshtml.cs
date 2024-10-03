using CourseHub.Core.Helpers.Validation;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.User;

public class RegisterModel : PageModel
{
    [BindProperty]
    public CreateUserDto Dto { get; set; }
    [BindProperty]
    public string RePassword { get; set; }


    public async Task OnPost([FromServices] IUserApiService userApiService)
    {
        if (!ModelState.IsValid)
            return;
        if (RePassword != Dto.Password)
        {
            ModelState.AddModelError("RePassword", "Password and RePassword don't match");
            return;
        }

        var response = await userApiService.RegisterAsync(Dto);

        TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.Conflict)
                TempData[Global.ALERT_MESSAGE] = "Cannot register!";
            else
                TempData[Global.ALERT_MESSAGE] = (await response.Content.ReadAsStringAsync()).Trim('\"');
            return;
        }
        TempData[Global.ALERT_MESSAGE] = "Please check your confirmation email!";
    }
}
