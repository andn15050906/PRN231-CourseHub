using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Course;

public class UpdateModel : PageModel
{
    private readonly ICourseApiService _courseApiService;


    // In each forms without CourseId
    [BindProperty]
    public Guid CourseId { get; set; }

    public CourseModel? Course { get; set; }

    public List<Category> Categories { get; set; }



    // UpdateCourse
    [BindProperty]
    public UpdateCourseDto UpdateCourseDto { get; set; }

    // Delete Section
    public string DeleteSectionPath { get; set; }



    public UpdateModel(ICourseApiService courseApiService)
    {
        _courseApiService = courseApiService;
    }



    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var client = context.HttpContext.GetClientData().Result;
        if (client is null)
            context.Result = Redirect(Global.PAGE_SIGNIN);
        else if (client.Role != Core.Entities.UserDomain.Enums.Role.Instructor)
            context.Result = Redirect(Global.PAGE_403);

        DeleteSectionPath = Configurer.GetApiClientOptions().ApiServerPath + "/api/courses";
    }

    public async Task<IActionResult> OnGet(
        [FromQuery] Guid courseId,
        [FromServices] ICategoryApiService categoryApiService)
    {
        if (courseId == default)
            return Redirect(Global.PAGE_404);
        Course = await _courseApiService.GetAsync(courseId);
        if (Course is null)
            return Redirect(Global.PAGE_404);

        Categories = await categoryApiService.GetAsync();

        if (courseId != default)
        {
            UpdateCourseDto = new UpdateCourseDto { Id = courseId };
        }

        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
	}

    public async Task<IActionResult> OnPostUpdateCourse()
    {
        //... Not model state
        if (string.IsNullOrEmpty(UpdateCourseDto.Title))
            return Reload(false, "Title cannot be empty!");
        if (UpdateCourseDto.DiscountExpiry is not null && UpdateCourseDto.DiscountExpiry < DateTime.UtcNow)
            return Reload(false, "Invalid discount expiry date!");
        if (UpdateCourseDto.Discount is not null && UpdateCourseDto.Discount > 0 &&
            UpdateCourseDto.DiscountExpiry is null)
            return Reload(false, "Invalid discount");
        if (UpdateCourseDto.Thumb is not null)
        {
            var file = UpdateCourseDto.Thumb.File;
            if (file is null)
                return Reload(false, "Invalid file!");
            if (!ResourceHelper.IsImage(file))
                return Reload(false, "Invalid file type!");
        }



        if (UpdateCourseDto.Discount is null)
        {
            //...
            UpdateCourseDto.Discount = 0;
        }
        if (string.IsNullOrEmpty(UpdateCourseDto.Outcomes))
            UpdateCourseDto.Outcomes = " ";
        if (string.IsNullOrEmpty(UpdateCourseDto.Requirements))
            UpdateCourseDto.Requirements = " ";




        var response = await _courseApiService.UpdateAsync(UpdateCourseDto, HttpContext);

        if (!response.IsSuccessStatusCode)
            return Reload(false, "Cannot update course!");

        return Reload(true, "Update course successfully!");
    }

    private IActionResult Reload(bool status, string message)
    {
        TempData[Global.ALERT_STATUS] = status;
        TempData[Global.ALERT_MESSAGE] = message;
        return Redirect(Request.Path + $"?courseId={UpdateCourseDto.Id}");
    }
}
