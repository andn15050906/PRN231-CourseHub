using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.UI.Pages.User;

public class ForgotPasswordModel : PageModel
{
    [BindProperty]
    [EmailAddress]
    public string Email { get; set; }

    public async Task OnPost([FromServices] IUserApiService userApiService)
    {
        if (!ModelState.IsValid)
            return;

        var response = await userApiService.RequestPasswordResetAsync(Email);

        TempData[Global.ALERT_MESSAGE] = response.IsSuccessStatusCode
            ? "Passsword reset email was sent."
            : "Could not send email!";
        TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;

        Email = string.Empty;
    }
}
