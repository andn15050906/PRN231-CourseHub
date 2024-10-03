using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.UserServices;
using CourseHub.UI.Services.Implementations.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.User;

public class ProfileModel : PageModel
{
    private readonly IUserApiService _userApiService;

    public UserFullModel? Client { get; set; }
    public string Avatar { get; set; }

    public string InstructorRequestPath { get; set; }

    [BindProperty]
    public UpdateUserDto Dto { get; set; }

    public ProfileModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
        //...
        InstructorRequestPath = Configurer.GetApiClientOptions().ApiServerPath + "/api/instructors";
    }
    
    public async Task<IActionResult> OnGet()
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        Avatar = UserApiService.GetAvatarApiUrl(Client.AvatarUrl, Client.Id);
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task OnPost()
    {
        var response = await _userApiService.UpdateAsync(Dto, HttpContext);
        TempData[Global.ALERT_MESSAGE] = response.IsSuccessStatusCode ?
            "Updated successfully." :
            $"Error updating!";
        TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;

        Client = (await HttpContext.GetClientData())!;
        Avatar = UserApiService.GetAvatarApiUrl(Client.AvatarUrl, Client.Id);
        TempData[Global.DATA_USE_BACKGROUND] = true;
    }
}
