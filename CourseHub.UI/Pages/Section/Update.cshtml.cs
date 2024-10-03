using CourseHub.Core.Entities.CourseDomain.Enums;
using CourseHub.Core.RequestDtos.Course.LectureDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseHub.UI.Services.Contracts.CourseServices;

namespace CourseHub.UI.Pages.Section;

public class UpdateModel : PageModel
{
    // CreateLecture
    [BindProperty]
    public CreateLectureDto CreateLectureDto { get; set; }
    [BindProperty]
    public IFormFile[] Files { get; set; }



    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var client = context.HttpContext.GetClientData().Result;
        if (client is null || client.Role < Core.Entities.UserDomain.Enums.Role.Instructor)
            context.Result = Redirect(Global.PAGE_404);
    }

    public async Task<IActionResult> OnGet([FromQuery] Guid sectionId)
    {
        if (sectionId == default)
            return Redirect(Global.PAGE_404);

        CreateLectureDto = new CreateLectureDto { SectionId = sectionId };

        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task<IActionResult> OnPostCreateLecture([FromServices] ILectureApiService lectureApiService)
    {
        CreateLectureDto.Materials = new();
        for (int i = 0; i < Files.Length; i++)
        {
            // prevent > 50MB
            if (Files[i].Length > 50000000)
            {
                TempData[Global.ALERT_STATUS] = false;
                TempData[Global.ALERT_MESSAGE] = "File size is too big";
                return Redirect(Request.Path + "?sectionId=" + CreateLectureDto.SectionId);
            }


            CreateLectureDto.Materials.Add(new CreateLectureDto.CreateLectureMaterialDto
            {
                Type = ResourceHelper.IsVideo(Files[i]) ? LectureMaterialType.Video : LectureMaterialType.Document,
                File = Files[i]
            });
        }

        if (string.IsNullOrWhiteSpace(CreateLectureDto.Content))
            CreateLectureDto.Content = CreateLectureDto.Title;

        var response = await lectureApiService.Create(CreateLectureDto, HttpContext);

        if (!response.IsSuccessStatusCode)
        {
            TempData[Global.ALERT_STATUS] = false;
            TempData[Global.ALERT_MESSAGE] = "Cannot create lecture!";
        }
        else
        {
            TempData[Global.ALERT_STATUS] = true;
            TempData[Global.ALERT_MESSAGE] = "Created lecture!";
        }

        Guid sectionId = CreateLectureDto.SectionId;
        return Redirect(Request.Path + "?sectionId=" + sectionId);
    }
}
