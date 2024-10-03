using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.User;

public class SignOutModel : PageModel
{
    private readonly IUserApiService _userApiService;

    public SignOutModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    public async Task<IActionResult> OnGet() => await HandleSignOut();

    public async Task<IActionResult> OnPost() => await HandleSignOut();

    private async Task<IActionResult> HandleSignOut()
    {

#pragma warning disable CS4014
        _userApiService.SignOutAsync();
#pragma warning restore CS4014

        Response.ExpireAuthCookies();
        HttpContext.Session.Clear();

        return RedirectToPage(Global.PAGE_INDEX);
    }
}
