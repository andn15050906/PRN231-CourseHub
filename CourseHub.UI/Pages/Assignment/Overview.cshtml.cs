using CourseHub.Core.Entities.AssignmentDomain;
using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.Models.Assignment.SubmissionModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Assignment;

public class OverviewModel : PageModel
{
    private readonly IAssignmentApiService _assignmentApiService;
    
    public AssignmentMinModel Assignment { get; set; }
    public UserFullModel Client { get; set; }
    public List<SubmissionMinModel> Submissions { get; set; }



    public OverviewModel(IAssignmentApiService assignmentApiService)
    {
        _assignmentApiService = assignmentApiService;
    }



    public async Task<IActionResult> OnGet([FromQuery] Guid id, [FromServices] ISubmissionApiService submissionApiService)
    {
#pragma warning disable CS8601
        Assignment = await _assignmentApiService.GetMinAsync(id);
        Client = await HttpContext.GetClientData();
#pragma warning restore CS8601



        if (Client is null)
            return Redirect(Global.PAGE_SIGNIN);
        if (Assignment is null)
            return Redirect(Global.PAGE_404);



        Submissions = await submissionApiService.GetByAssignmentAsync(id, HttpContext);



        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}


