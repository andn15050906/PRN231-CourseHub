using CourseHub.Core.Models.User.UserModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Pages.User;

public class ExternalRedirectModel : PageModel
{
    public IActionResult OnGet(string value)
    {
        var data = JsonSerializer.Deserialize<UserFullModel>(value, SerializeOptions.JsonOptions);
        if (data is not null)
            HttpContext.SetClientData(value);
        return Redirect(Global.PAGE_INDEX);
    }
}
