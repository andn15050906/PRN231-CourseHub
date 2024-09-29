using CourseHub.Core.Services.Domain.UserServices;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.User;

public class VerifyEmailModel : PageModel
{
    private readonly IUserApiService _userService;

    public VerifyEmailModel(IUserApiService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> OnGet([FromRoute] string email, [FromRoute] string token)
    {
        var response = await _userService.VerifyEmailAsync(email, token);
        if (response.IsSuccessStatusCode)
            return Redirect(Global.PAGE_SIGNIN);

        return Page();
    }
}
