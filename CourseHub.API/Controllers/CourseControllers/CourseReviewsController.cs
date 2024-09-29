using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Course.CourseReviewDtos;
using CourseHub.Core.Services.Domain.CourseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CourseHub.API.Controllers.CourseControllers;

[Produces(contentType: MediaTypeNames.Application.Json)]
public class CourseReviewsController : BaseController
{
    private readonly ICourseReviewService _courseReviewService;

    public CourseReviewsController(ICourseReviewService courseReviewService)
    {
        _courseReviewService = courseReviewService;
    }



    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get([FromQuery] QueryCourseReviewDto dto)
    {
        var result = await _courseReviewService.GetPagedAsync(dto);
        return result.AsResponse();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateCourseReviewDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _courseReviewService.CreateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateCourseReviewDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _courseReviewService.UpdateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _courseReviewService.DeleteAsync(id, clientId);
        return result.AsResponse();
    }
}
