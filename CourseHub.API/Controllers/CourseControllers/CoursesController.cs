using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.Core.Services.Domain.CourseServices;
using CourseHub.Core.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers.CourseControllers;

public class CoursesController : BaseController
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }



    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetOverview([FromQuery] QueryCourseDto dto)
    {
        var result = await _courseService.GetPagedAsync(dto);
        return result.AsResponse();
    }

    [HttpGet("multiple")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetMultiple([FromQuery] Guid[] ids)
    {
        var result = await _courseService.GetMultiple(ids);
        return result.AsResponse();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _courseService.GetAsync(id);
        return result.AsResponse();
    }

    [HttpGet("Resource/{*media}")]
    [ResponseCache(Duration = 60)]
    public IActionResult GetResource(string media)
    {
        if (IsRemote(media))
            return Ok(media);

        Stream? stream = ServerStorage.ReadAsStream(CourseStorage.GetCourseMediaPath(media));
        return stream is null ? NotFound() : new FileStreamResult(stream, "image/jpeg");
    }

    [HttpGet("{id}/similar")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetSimilar(Guid id)
    {
        var result = await _courseService.GetAsync(id);
        return result.AsResponse();
    }

    // For suggestions
    [HttpGet("minimum")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetMinimum([FromQuery] QueryCourseDto dto)
    {
        var result = await _courseService.GetMin(dto);
        return result.AsResponse();
    }






    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCourseDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _courseService.CreateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateCourseDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _courseService.UpdateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _courseService.DeleteAsync(id, clientId);
        return result.AsResponse();
    }






    private static bool IsRemote(string media)
    {
        return media.StartsWith("http");
    }
}
