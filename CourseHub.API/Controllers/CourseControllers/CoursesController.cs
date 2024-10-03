using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Course.CourseDtos;
using CourseHub.Core.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using CourseHub.Core.Services.Domain.CourseServices;

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
    public async Task<IActionResult> GetPaged([FromQuery] QueryCourseDto dto)
    {
        var result = await _courseService.GetPagedAsync(dto);
        return result.AsResponse();
    }

    [HttpGet("multiple")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetMultiple([FromQuery] Guid[] ids)
    {
        var result = await _courseService.GetMultipleAsync(ids);
        return result.AsResponse();
    }

    [HttpGet("{id}")]
    //[ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _courseService.GetAsync(id);
        return result.AsResponse();
    }

    [HttpGet("BySection")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetBySection([FromQuery] Guid sectionId)
    {
        var result = await _courseService.GetBySectionAsync(sectionId);
        return result.AsResponse();
    }


    [HttpGet("Resource/{courseId}/local-thumb")]
    [ResponseCache(Duration = 60)]
    public IActionResult GetLocalThumb(Guid courseId)
    {
        string path = CourseStorage.GetCourseThumbPath(courseId);
        Stream? stream = ServerStorage.ReadAsStream(path);
        return stream is null ? NotFound() : new FileStreamResult(stream, "image/jpeg");
    }

    /*[HttpGet("Resource/Media/{*path}")]
    [ResponseCache(Duration = 60)]
    public IActionResult GetMedia(string path)
    {
        //... Authorize -> Enrollment where courseId && clientId
        var result = ServerStorage.ReadAsStreamWithName(path);
        return result.Item1 is null
            ? NotFound()
            : new FileStreamResult(result.Item1, MimeTypes.GetMimeType(result.Item2!));
    }*/

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
        var result = await _courseService.GetMinAsync(dto);
        return result.AsResponse();
    }






    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCourseDto dto)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _courseService.CreateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateCourseDto dto)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _courseService.UpdateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _courseService.DeleteAsync(id, clientId);
        return result.AsResponse();
    }
}
