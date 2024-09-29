using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Common.CommentModels;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Common.CommentDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.CommonServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Implementations.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Lecture;

public class DetailModel : PageModel
{
    public Core.Entities.CourseDomain.Lecture Lecture { get; set; }
    public UserFullModel Client { get; set; }
    public CourseOverviewModel Course { get; set; }
    public PagedResult<CommentModel> Comments { get; set; }
    public bool IsCreator { get; set; }



    public string CommentApiPath { get; set; }



    [BindProperty]
    public Guid Id { get; set; }
    [BindProperty]
    public CreateCommentDto CreateCommentDto { get; set; }



    public async Task<IActionResult> OnGet([FromQuery] Guid id,
        [FromServices] ILectureApiService lectureApiService, [FromServices] ICourseApiService courseApiService, [FromServices] ICommentApiService commentApiService)
    {
        Lecture = await lectureApiService.GetAsync(id, HttpContext);
        if (Lecture is null)
            return Redirect(Global.PAGE_404);

        Client = await HttpContext.GetClientData();
        if (!Lecture.IsPreviewable && Client is null)
            return Redirect(Global.PAGE_403);

        Course = await courseApiService.GetBySectionIdAsync(Lecture.SectionId);
        if (Course is null)
            return Redirect(Global.PAGE_404);

        if (Client is not null && Client.Id == Course.Creator.Id)
            IsCreator = true;
        if (!IsCreator)
        {
            var isEnrolled = await courseApiService.IsEnrolled(Course.Id, HttpContext);
            if (!Lecture.IsPreviewable && !isEnrolled)
                return Redirect(Global.PAGE_403);
        }

        QueryCommentDto dto = new()
        {
            LectureId = Lecture.Id
        };
        Comments = await commentApiService.GetAsync(dto);

        Course.Creator.AvatarUrl = UserApiService.GetAvatarApiUrl(Course.Creator.AvatarUrl, Course.Id);
        foreach (var comment in Comments.Items)
        {
            if (comment.Creator is not null)
                comment.Creator.AvatarUrl = UserApiService.GetAvatarApiUrl(comment.Creator.AvatarUrl, comment.Creator.Id);
        }

        CommentApiPath = Configurer.GetApiClientOptions().ApiServerPath + "/api/comments";
        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }

    public async Task<IActionResult> OnPostCreateComment(
        [FromServices] ICommentApiService commentApiService)
    {
        await commentApiService.CreateAsync(CreateCommentDto, HttpContext);

        return Redirect(Request.Path + $"?id={Id}");
    }
}
