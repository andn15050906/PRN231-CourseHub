using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;
using CourseHub.Core.RequestDtos.Assignment.McqQuestionDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Pages.Assignment;

public class CreateModel : PageModel
{
    public CourseOverviewModel Course { get; set; }

    [BindProperty]
    public CreateAssignmentDto Dto { get; set; }
    [BindProperty]
    public Guid SectionId { get; set; }
    [BindProperty]
    public IFormFile File { get; set; }



    /*public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var client = context.HttpContext.GetClientData().Result;
        if (client is null || client.Role != Core.Entities.UserDomain.Enums.Role.Instructor)
            context.Result = Redirect(Global.PAGE_404);
    }*/



    public async Task<IActionResult> OnGet([FromQuery] Guid sectionId, [FromServices] ICourseApiService courseApiService)
    {
        if (sectionId == default)
            return Redirect(Global.PAGE_404);

        var course = await courseApiService.GetBySectionIdAsync(sectionId);
        if (course is null)
            return Redirect(Global.PAGE_404);

        Dto = new();

        SectionId = sectionId;
        Course = course;
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task<IActionResult> OnPost([FromServices] IAssignmentApiService assignmentApiService)
    {
        if (Dto.Name is null || Dto.Name.Length == 0)
            return RenderPage(false, "Assignment name is required");
        if (Dto.Name.Length > 255)
            return RenderPage(false, "Assignment name must be less than 255 characters");
        if (Dto.Duration < 10)
            return RenderPage(false, "Assignment duration is too short");
        if (Dto.GradeToPass < 0 || Dto.GradeToPass > 10)
            return RenderPage(false, "Grade to pass must be between 0 and 10");



        //...
        if (File is null || File.ContentType != "application/json")
            return RenderPage(false, "The file format is invalid");

        using var reader = new StreamReader(File.OpenReadStream());
        var content = await reader.ReadToEndAsync();
        List<CreateMcqQuestionDto>? questions = JsonSerializer.Deserialize<List<CreateMcqQuestionDto>>(content);

        //...
        if (questions is null)
            return RenderPage(false, "The file format is invalid");



        Dto.Questions = questions;
        var response = await assignmentApiService.CreateAsync(Dto, HttpContext);
        return RenderPage(true, "Created Assignment!");
    }

    private PageResult RenderPage(bool isSuccessful, string message)
    {
        TempData[Global.ALERT_STATUS] = isSuccessful;
        TempData[Global.ALERT_MESSAGE] = message;
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}
