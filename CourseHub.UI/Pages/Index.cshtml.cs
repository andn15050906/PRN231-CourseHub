using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
	private readonly ICourseApiService _courseApiService;

	public PagedResult<CourseOverviewModel> Courses { get; set; }

	public IndexModel(ILogger<IndexModel> logger, ICourseApiService courseApiService)
    {
        _logger = logger;
		_courseApiService = courseApiService;
    }

    public async Task OnGet()
	{
		QueryCourseDto dto = new();
		Courses = await _courseApiService.GetPagedAsync(dto);
	}
}