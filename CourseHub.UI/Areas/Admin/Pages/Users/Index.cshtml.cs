using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Areas.Admin.Pages.Users;

public class IndexModel : PageModel
{
    private IUserApiService _userService;

    public PagedResult<UserModel> Data { get; set; }

    public IndexModel(IUserApiService userService)
    {
        _userService = userService;
    }

    public async Task OnGet()
    {
        QueryUserDto dto = new();
        Data = await _userService.GetPagedAsync(dto, HttpContext);
    }
}
