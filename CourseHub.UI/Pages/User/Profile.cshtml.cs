using CourseHub.Core.Models.User.UserModels;
using CourseHub.UI.Services.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.User;

public class ProfileModel : PageModel
{
    private readonly IUserApiService _userApiService;

    public UserFullModel? Client { get; set; }


    public class ExModel {
        //...
    }

    public ProfileModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }
    
    public async Task OnGet()
    {
        Client = await _userApiService.GetClientAsync(HttpContext);
    }
}
