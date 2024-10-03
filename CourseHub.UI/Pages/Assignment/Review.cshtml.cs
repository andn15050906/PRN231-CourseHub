using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.Models.Assignment.SubmissionModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Assignment;

public class ReviewModel : PageModel
{
    public AssignmentModel Assignment { get; set; }
    public UserFullModel Client { get; set; }
    public SubmissionModel Submission { get; set; }
    public Dictionary<Guid, IEnumerable<Guid>> Question_Choice { get; set; }

    public async Task<IActionResult> OnGet(
        [FromQuery] Guid assignmentId, [FromQuery] Guid submissionId,
        [FromServices] IAssignmentApiService assignmentApiService, [FromServices] ISubmissionApiService submissionApiService)
    {
        Client = await HttpContext.GetClientData();
        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);

        var assignmentTask = assignmentApiService.GetAsync(assignmentId, HttpContext);
        var submissionTask = submissionApiService.GetAsync(submissionId, HttpContext);
        await Task.WhenAll(assignmentTask, submissionTask);
#pragma warning disable CS8601
        Assignment = assignmentTask.Result;
        Submission = submissionTask.Result;
#pragma warning restore CS8601
        if (Assignment is null || Submission is null ||
            Submission.AssignmentId != Assignment.Id || Submission.CreatorId != Client.Id)
            return Redirect(Global.PAGE_404);

        Question_Choice = new();
        foreach (var question in Assignment.Questions)
        {
            var answers = question.Choices.Where(_ => _.IsCorrect).Select(_ => _.Id).ToList();
            Question_Choice.Add(question.Id, answers);
        }

        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}
