using CourseHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Shared;

public class _403Model : PageModel
{
    public void OnGet()
    {
        TempData[Global.DATA_USE_BACKGROUND] = true;
    }
}
