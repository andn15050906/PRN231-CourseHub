using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;
using CourseHub.Core.RequestDtos.Assignment.McqQuestionDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Pages.Assignment;

public class UpdateModel : PageModel
{
    private readonly IAssignmentApiService _assignmentApiService;

    public AssignmentModel Assignment { get; set; }
    public string DeleteSectionPath { get; set; }

    [BindProperty]
    public UpdateAssignmentDto Dto { get; set; }
    [BindProperty]
    public IFormFile File { get; set; }



    public UpdateModel(IAssignmentApiService assignmentApiService)
    {
        _assignmentApiService = assignmentApiService;
    }


    public async Task<IActionResult> OnGet([FromQuery] Guid id)
    {
        if (id == default)
            return Redirect(Global.PAGE_404);

        var assignment = await _assignmentApiService.GetAsync(id, HttpContext);
        if (assignment is null)
            return Redirect(Global.PAGE_404);

        Assignment = assignment;
        TempData[Global.DATA_USE_BACKGROUND] = true;
        DeleteSectionPath = Configurer.GetApiClientOptions().ApiServerPath + "/api/assignments";
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Dto.Name is null || Dto.Name.Length == 0)
            return RenderAndRedirect(false, "Assignment name is required");
        if (Dto.Name.Length > 255)
            return RenderAndRedirect(false, "Assignment name must be less than 255 characters");
        if (Dto.Duration < 10)
            return RenderAndRedirect(false, "Assignment duration is too short");
        if (Dto.GradeToPass < 0 || Dto.GradeToPass > 10)
            return RenderAndRedirect(false, "Grade to pass must be between 0 and 10");

        if (File is not null && File.ContentType != "application/json")
            return RenderAndRedirect(false, "The file format is invalid");
        using var reader = new StreamReader(File.OpenReadStream());
        var content = await reader.ReadToEndAsync();
        List<CreateMcqQuestionDto>? questions = JsonSerializer.Deserialize<List<CreateMcqQuestionDto>>(content);
        if (questions is null)
            return RenderAndRedirect(false, "The file format is invalid");
        //...
        var response = await _assignmentApiService.UpdateAsync(Dto, HttpContext);
        if (!response.IsSuccessStatusCode)
            return RenderAndRedirect(false, "Cannot update assignment!");

        return RenderAndRedirect(true, "Updated Assignment Successfully!");
    }



    private IActionResult RenderAndRedirect(bool isSuccessful, string message)
    {
        TempData[Global.ALERT_STATUS] = isSuccessful;
        TempData[Global.ALERT_MESSAGE] = message;
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Redirect(Request.Path + "?id=" + Dto.Id);
    }
}
