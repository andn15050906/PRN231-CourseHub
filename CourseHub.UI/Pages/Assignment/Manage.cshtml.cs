using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Assignment;

public class ManageModel : PageModel
{
    private readonly IAssignmentApiService _assignmentApiService;

    public List<AssignmentMinModel> Assignments { get; set; }
    public CourseModel Course { get; set; }



    public ManageModel(IAssignmentApiService assignmentApiService)
    {
        _assignmentApiService = assignmentApiService;
    }

    public async Task<IActionResult> OnGet([FromQuery] Guid courseId,
        [FromServices] ICourseApiService courseApiService, [FromServices] IInstructorApiService instructorApiService)
    {
        if (courseId == default)
            return Redirect(Global.PAGE_404);

        var client = await HttpContext.GetClientData();
        if (client is null)
            return Redirect(Global.PAGE_SIGNIN);

        var instructor = await instructorApiService.GetByUserId(client.Id);
        if (instructor is null)
            return Redirect(Global.PAGE_403);

        var courseTask = courseApiService.GetAsync(courseId);
        var assignmentTask = _assignmentApiService.GetByCourseAsync(courseId);
        await Task.WhenAll(courseTask, assignmentTask);
        
        Course = courseTask.Result;
        Assignments = assignmentTask.Result;
        if (Course is null)
            return Redirect(Global.PAGE_404);
        if (Course.InstructorId != instructor.Id)
            return Redirect(Global.PAGE_403);



        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}
