using CourseHub.Core.Helpers.Validation;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.UI.Pages.User;

public class ChangePasswordModel : PageModel
{
    [BindProperty]
    [PasswordValidation]
    public string? CurrentPassword { get; set; }

    [BindProperty]
    [PasswordValidation]
    public string? NewPassword { get; set; }

    [BindProperty]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
    public string? RePassword { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var data = await HttpContext.GetClientData();
        if (data is null)
            return Redirect(Global.PAGE_SIGNIN);
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task OnPost([FromServices] IUserApiService userApiService)
    {
        if (NewPassword == CurrentPassword)
        {
            ModelState.AddModelError(nameof(NewPassword), "New Password must be different from Current Password");
        }

        if (!ModelState.IsValid)
        {
            TempData[Global.DATA_USE_BACKGROUND] = true;
            return;
        }

        UpdateUserDto dto = new()
        {
            CurrentPassword = CurrentPassword,
            NewPassword = NewPassword
        };

        var response = await userApiService.UpdateAsync(dto, HttpContext);

        TempData[Global.ALERT_MESSAGE] = response.IsSuccessStatusCode
            ? "Updated successfully."
            : "Invalid password!";
        TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return;
    }
}
