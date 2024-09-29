using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace CourseHub.UI.Pages.User;

public class SignInModel : PageModel
{
    private readonly IUserApiService _userApiService;

    [BindProperty]
    public SignInDto SignInDto { get; set; }                // the page uses username only
    [BindProperty]
    public bool RememberMe { get; set; }

    public SignInModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }



    public async Task<IActionResult> OnPostAsync()
    {
        HttpResponseMessage response = await _userApiService.SignInAsync(SignInDto);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Lỗi đăng nhập.");
            return Page();
        }

        // set userData in the session
        // if "RememberMe" is true, set userId in the cookie

        string sResponse = await response.Content.ReadAsStringAsync();
        UserFullModel user = JsonSerializer.Deserialize<UserFullModel>(sResponse)!;
        HttpContext.SetClientData(sResponse);

        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            foreach (var header in SetCookieHeaderValue.ParseList(cookies.ToList()))
                if (header.Name == "Bearer" || header.Name == "Refresh")
                    HttpContext.Response.SetAuthCookie(header.Name.ToString(), header.Value.ToString());
        }

        if (RememberMe)
            Response.AddRememberCookie(user.Id);
        return RedirectToPage(Global.PAGE_INDEX);
    }
}
