using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Assignment.SubmissionDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Assignment;

public class AttemptModel : PageModel
{
    public AssignmentModel Assignment { get; set; }
    public UserFullModel Client { get; set; }
    public CourseOverviewModel Course { get; set; }

    [BindProperty]
    public CreateSubmissionDto Dto { get; set; }
    [BindProperty]
    public Guid[] AnswerChoices { get; set; }



    public async Task<IActionResult> OnGet(
        [FromQuery] Guid id,
        [FromServices] IAssignmentApiService assignmentApiService, [FromServices] ICourseApiService courseApiService)
    {
#pragma warning disable CS8601
        Assignment = await assignmentApiService.GetAsync(id, HttpContext);
        Client = await HttpContext.GetClientData();

        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);
        if (Assignment is null || Assignment.Section is null)
            return Redirect(Global.PAGE_404);

        Course = await courseApiService.GetBySectionIdAsync(Assignment.Section.Id);
#pragma warning restore CS8601



        if (Course is null)
            return Redirect(Global.PAGE_404);
        var isEnrolled = await courseApiService.IsEnrolled(Course.Id, HttpContext);
        if (!isEnrolled)
            return Redirect(Global.PAGE_COURSE_DETAIL + "?id=" + Course.Id);



        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task<IActionResult> OnPost([FromServices] ISubmissionApiService submissionApiService)
    {
        Dto.Answers = AnswerChoices
            .Select(_ => new CreateSubmissionDto.CreateMcqUserAnswerDto { MCQChoiceId = _ })
            .ToList();

        /*if (!ModelState.IsValid)
        {
            foreach (var state in ModelState)
                System.Diagnostics.Debug.WriteLine(state.Value.Errors);
            TempData[Global.DATA_USE_BACKGROUND] = true;
            //...
            return Redirect(Request.Path);
        }*/

        var response = await submissionApiService.CreateAsync(Dto, HttpContext);
        if (!response.IsSuccessStatusCode)
        {
            TempData[Global.DATA_USE_BACKGROUND] = true;
            //...
            return Redirect(Request.Path + $"?id={Dto.AssignmentId}");
        }

        return Redirect($"{Global.PAGE_ASSIGNMENT_OVERVIEW}?id={Dto.AssignmentId}");
    }
}
