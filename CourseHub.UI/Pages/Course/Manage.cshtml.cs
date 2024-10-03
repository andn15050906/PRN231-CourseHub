using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.UI.Services.Contracts.CourseServices;

namespace CourseHub.UI.Pages.Course;

public class ManageModel : PageModel
{
    public UserFullModel? Client { get; set; }
    public InstructorModel? Instructor { get; set; }
    public PagedResult<CourseOverviewModel> Courses { get; set; }
    public List<Category> Categories { get; set; }

    public async Task<IActionResult> OnGet(
        [FromServices] ICourseApiService courseApiService,
        [FromServices] IInstructorApiService instructorApiService,
        [FromServices] ICategoryApiService categoryApiService)
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        Instructor = await instructorApiService.GetByUserId(Client.Id);
        if (Instructor is null)
            return Redirect(Global.PAGE_403);

        QueryCourseDto query = new()
        {
            InstructorId = Instructor.Id
        };
        var coursesTask = courseApiService.GetPagedAsync(query);
        var categoriesTask = categoryApiService.GetAsync();

        await Task.WhenAll(coursesTask, categoriesTask);
        Courses = coursesTask.Result;
        Categories = categoriesTask.Result;
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}
