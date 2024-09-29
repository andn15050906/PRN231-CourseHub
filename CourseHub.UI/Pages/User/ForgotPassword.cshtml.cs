using CourseHub.UI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.UI.Pages.User;

public class ForgotPasswordModel : PageModel
{
    [BindProperty]
    [EmailAddress]
    public string Email { get; set; }

    public void OnGet()
    {
    }

    public void OnPost([FromServices] IUserApiService userApiService)
    {
        if (!ModelState.IsValid)
            return;

        userApiService.RequestPasswordResetAsync(Email);
    }
}
