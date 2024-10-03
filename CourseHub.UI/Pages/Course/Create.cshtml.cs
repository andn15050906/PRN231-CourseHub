using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Course;

public class CreateModel : PageModel
{
    [BindProperty]
    public CreateCourseDto Dto { get; set; }
    [BindProperty]
    public IFormFile[] Files { get; set; }

    public List<Category> Categories { get; set; }



    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var client = context.HttpContext.GetClientData().Result;
        if (client is null || client.Role != Core.Entities.UserDomain.Enums.Role.Instructor)
            context.Result = Redirect(Global.PAGE_404);
    }

    public async Task OnGet([FromServices] ICategoryApiService categoryApiService)
    {
        Categories = await categoryApiService.GetAsync();
        System.Diagnostics.Debug.WriteLine(Categories);
        TempData[Global.DATA_USE_BACKGROUND] = true;
    }

    //...
    public async Task<IActionResult> OnPost(
		[FromServices] ICategoryApiService categoryApiService,
		[FromServices] ICourseApiService courseApiService)
	{
		TempData[Global.DATA_USE_BACKGROUND] = true;
		Categories = await categoryApiService.GetAsync();

        if (!ModelState.IsValid)
        {
            TempData[Global.ALERT_STATUS] = false;

            if (Dto.SectionNames is null || Dto.SectionNames.Count == 0)
                TempData[Global.ALERT_MESSAGE] = "Sections are required!";
            else
                TempData[Global.ALERT_MESSAGE] = "Invalid fields";

			return Page();
        }



        var response = await courseApiService.CreateAsync(Dto, HttpContext);

		TempData[Global.ALERT_STATUS] = response.IsSuccessStatusCode;
		if (!response.IsSuccessStatusCode)
        {
            TempData[Global.ALERT_MESSAGE] = "Cannot create course!";
			return Page();
        }

		TempData[Global.ALERT_MESSAGE] = "Create course successfully!";
		return Page();
    }
}
