using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Areas.Admin.Pages.Courses;

public class IndexModel : PageModel
{
    public Dictionary<Guid, CourseMinModel> Courses { get; set; }
    public Dictionary<Guid, UserMinModel> Users { get; set; }
    public UserFullModel? Client { get; set; }

    //[BindProperty]
    //public string Action { get; set; }



    public async Task OnGet(
        [FromServices] IUserApiService userApiService,
        [FromServices] ICourseApiService courseApiService)
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
        {
            Redirect(Global.PAGE_SIGNIN);
            return;
        }

        QueryCourseDto dto = new() { PageSize = 30 };
        var courses = (await courseApiService.GetMinAsync(dto))?.Items ?? new List<CourseMinModel>();
        Courses = courses.ToDictionary(_ => _.Id);

        Users = (await userApiService.GetAllMinAsync(HttpContext)).ToDictionary(_ => _.Id);
    }
}
