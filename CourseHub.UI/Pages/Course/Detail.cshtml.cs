using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Assignment.AssignmentModels;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.Models.Course.CourseReviewModels;
using CourseHub.Core.Models.Course.EnrollmentModels;
using CourseHub.Core.Models.Course.InstructorModels;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Services.Contracts.AssignmentServices;
using CourseHub.UI.Services.Contracts.CourseServices;
using CourseHub.UI.Services.Contracts.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CourseHub.UI.Pages.Course;

public class DetailModel : PageModel
{
    private readonly ICourseApiService _courseApiService;



    public UserFullModel? Client { get; set; }
    public CourseModel? Course { get; set; }
    public InstructorModel? Instructor { get; set; } = new InstructorModel { };
    public UserModel? InstructorUser { get; set; } = new UserModel { };
    public bool IsEnrolled { get; set; }
    public bool IsCreator { get; set; }
    public List<AssignmentMinModel> Assignments { get; set; }
    public EnrollmentFullModel Enrollment { get; set; }
    public List<Guid> AssignmentMilestones { get; set; }



    public PagedResult<CourseReviewModel> Reviews { get; set; }
    public List<CourseOverviewModel> MoreCourses { get; set; } = new();
    public List<UserMinModel> RelatedUsers { get; set; }
    public string ReportPath { get; set; }
    public string ReviewPath { get; set; }



    // Common for forms
    [BindProperty]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateCourseReviewDto CreateCourseReviewDto { get; set; }






    public DetailModel(ICourseApiService courseApiService)
    {
        _courseApiService = courseApiService;
    }

    public async Task<IActionResult> OnGet(
        [FromQuery] Guid id,
        [FromServices] IAssignmentApiService assignmentApiService,
        [FromServices] IUserApiService userApiService, [FromServices] ICourseReviewApiService reviewApiService)
    {
        return await RenderPage(id, assignmentApiService, userApiService, reviewApiService);
    }

	public async Task<IActionResult> OnPostCreateReview(
        [FromServices] ICourseReviewApiService reviewApiService)
	{
        await reviewApiService.CreateAsync(CreateCourseReviewDto, HttpContext);

        return Redirect(Request.Path + $"?id={Id}");
    }






    private async Task<IActionResult> RenderPage(
        Guid id,
        IAssignmentApiService assignmentApiService,
        IUserApiService userApiService, ICourseReviewApiService reviewApiService)
    {
        if (id == default)
            return Redirect(Global.PAGE_404);

        var courseTask = _courseApiService.GetAsync(id);
        var clientTask = HttpContext.GetClientData();
        await Task.WhenAll(courseTask, clientTask);
        Course = courseTask.Result;
        Client = clientTask.Result;
        if (Course is null)
            return Redirect(Global.PAGE_404);

        if (Client is not null && Client.Id == Course.Creator.Id)
            IsCreator = true;

        //...
        Course.Reviews = new();
        var instructorUserTask = userApiService.GetAsync(Course.Creator.Id);
        var isEnrolledTask = _courseApiService.IsEnrolled(Course.Id, HttpContext);
        var assignmentsTask = assignmentApiService.GetBySectionsAsync(Course.Sections.Select(_ => _.Id));
        var reviewsTask = reviewApiService.GetAsync(new QueryCourseReviewDto { CourseId = Course.Id });
        var moreCoursesTask = _courseApiService.GetPagedAsync(
            new Core.RequestDtos.Course.CourseDtos.QueryCourseDto
            {
                PageSize = 4,
                InstructorId = Course.InstructorId
            }
        );

        await Task.WhenAll(instructorUserTask, isEnrolledTask, assignmentsTask, reviewsTask, moreCoursesTask);
        InstructorUser = instructorUserTask.Result;
        IsEnrolled = isEnrolledTask.Result;
        Assignments = assignmentsTask.Result;
        Reviews = reviewsTask.Result;
        MoreCourses = moreCoursesTask.Result.Items.Where(_ => _.Id != Course.Id).ToList();
        
        if (IsEnrolled)
        {
            var enrollment = await _courseApiService.GetEnrollmentAsync(HttpContext, Course.Id);
            if (enrollment is not null)
            {
                Enrollment = enrollment;
                if (!string.IsNullOrEmpty(enrollment.AssignmentMilestones))
                    AssignmentMilestones = JsonSerializer.Deserialize<List<Guid>>(enrollment.AssignmentMilestones);
            }
        }

        if (Reviews.TotalCount > 0)
        {
            RelatedUsers = await userApiService.GetMinAsync(Reviews.Items.Select(_ => _.CreatorId).Distinct());
        }

        var apiServerPath = Configurer.GetApiClientOptions().ApiServerPath;
        ReportPath = apiServerPath + "/api/notifications";
        ReviewPath = apiServerPath + "/api/CourseReviews";

        TempData[Global.DATA_USE_BACKGROUND] = true;
        return Page();
    }
}
