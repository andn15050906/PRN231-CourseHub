using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.UserServices;
using CourseHub.UI.Services.Implementations.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace CourseHub.UI.Areas.Admin.Pages.CreateAdmin;

public class IndexModel : PageModel
{
    [BindProperty]
    public CreateUserDto Dto { get; set; }
    [BindProperty]
    public string RePassword { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost([FromServices] IUserApiService userApiService)
    {
        if (!ModelState.IsValid)
        {
            TempData[Global.ALERT_STATUS] = false;
            StringBuilder errorMessage = new();
            foreach (var stateValue in ModelState.Values)
                foreach (var error in stateValue.Errors)
                {
                    if (error.ErrorMessage.StartsWith("400") ||
                        error.ErrorMessage.StartsWith("401") ||
                        error.ErrorMessage.StartsWith("403"))
                        errorMessage.Append(error.ErrorMessage.Substring(5)).Append("\n");
                }
            if (errorMessage.Length > 0)
                TempData[Global.ALERT_MESSAGE] = errorMessage.ToString();
            return Redirect(Request.Path);
        }
        if (RePassword != Dto.Password)
        {
            TempData[Global.ALERT_STATUS] = false;
            TempData[Global.ALERT_MESSAGE] = "Password and RePassword doesn't match";
            return Redirect(Request.Path);
        }

        var response = await userApiService.CreateAdminAsync(Dto, HttpContext);

        TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                TempData[Global.ALERT_MESSAGE] = (await response.Content.ReadAsStringAsync()).Trim('\"');
            }
            else
            {
                TempData[Global.ALERT_MESSAGE] = "Cannot create admin account!";
            }
            return Redirect(Request.Path);
        }

        TempData[Global.ALERT_MESSAGE] = "Created admin account successfully!";
        return Redirect(Request.Path);
    }
}
