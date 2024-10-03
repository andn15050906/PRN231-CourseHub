using CourseHub.Core.Entities.CourseDomain;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.Course.CourseModels;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.UI.Helpers;
using CourseHub.UI.Services.Contracts.CourseServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseHub.UI.Pages.Course;

public class SearchModel : PageModel
{
    private readonly ICourseApiService _courseApiService;
    private readonly ICategoryApiService _categoryApiService;

    public PagedResult<CourseOverviewModel> Courses { get; set; }
    public List<Category> Categories { get; set; }

    public SearchModel(ICourseApiService courseApiService, ICategoryApiService categoryApiService)
    {
        _courseApiService = courseApiService;
        _categoryApiService = categoryApiService;
    }

    public async Task OnGet(
        [FromQuery] string sortBy, [FromQuery] string? q = null,
        [FromQuery] Guid? category = null, [FromQuery] byte page = 0)
    {
        QueryCourseDto dto = new() { PageIndex = page, PageSize = 12 };
        switch (sortBy)
        {
            case "learnerCount":
                dto.ByLearnerCount = true;
                break;
            case "avgRating":
                dto.ByAvgRating = true;
                break;
            case "price":
                dto.ByPrice = true;
                break;
            case "discount":
                dto.ByDiscount = true;
                break;
            case "new":
                break;
        };
        if (category is not null)
            dto.CategoryId = (Guid)category;
        if (q is not null)
            dto.Title = q;
        
        var courseTask = _courseApiService.GetPagedAsync(dto);
        var categoryTask = _categoryApiService.GetAsync();

        await Task.WhenAll(courseTask, categoryTask);
        Courses = courseTask.Result;
        Categories = categoryTask.Result;

        TempData[Global.DATA_USE_BACKGROUND] = true;
    }
}
