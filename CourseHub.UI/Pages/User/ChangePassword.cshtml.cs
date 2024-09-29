using CourseHub.Core.Helpers.Validation;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts;
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

    /// <summary>
    /// Only for authenticated users
    /// </summary>
    public async Task<IActionResult> OnGet()
    {
        var data = await HttpContext.GetClientData();
        if (data is null)
            return Redirect(Global.PAGE_SIGNIN);
        return Page();
    }

    public async Task OnPost([FromServices] IUserApiService userApiService)
    {
        UpdateUserDto dto = new()
        {
            CurrentPassword = CurrentPassword,
            NewPassword = NewPassword
        };

        var response = await userApiService.UpdateAsync(dto, HttpContext);
        System.Diagnostics.Debug.WriteLine(response);
    }
}
