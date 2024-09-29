using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.User;

public class ResetPasswordModel : PageModel
{
    private readonly IUserApiService _userApiService;

    [BindProperty]
    public ResetPasswordDto ResetPasswordDto { get; set; }

    [BindProperty]
    public string RePassword { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }

    public ResetPasswordModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    public void OnGet([FromRoute] string email, [FromRoute] string token)
    {
        Email = email;
        Token = token;
    }

    public async Task<IActionResult> OnPost([FromRoute] string email, [FromRoute] string token)
    {
        if (RePassword != ResetPasswordDto.NewPassword)
        {
            ModelState.AddModelError(nameof(RePassword), "Mật khẩu không khớp");
            return ReDisplay(email, token);
        }

        var response = await _userApiService.ResetPasswordAsync(ResetPasswordDto);

        if (response.IsSuccessStatusCode)
            return Redirect(Global.PAGE_SIGNIN);

        return ReDisplay(email, token);
    }

    private IActionResult ReDisplay(string email, string token)
    {
        Email = email;
        Token = token;
        return Page();
    }
}
