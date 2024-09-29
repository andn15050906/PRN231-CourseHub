using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Course.LectureDtos;
using CourseHub.Core.Services.Domain.CourseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CourseHub.API.Controllers.CourseControllers;

[Produces(contentType: MediaTypeNames.Application.Json)]
public class LecturesController : BaseController
{
    private readonly ILectureService _lectureService;

    public LecturesController(ILectureService lectureService)
    {
        _lectureService = lectureService;
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _lectureService.GetAsync(id);
        return result.AsResponse();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateLectureDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _lectureService.CreateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateLectureDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _lectureService.UpdateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _lectureService.DeleteAsync(id, clientId);
        return result.AsResponse();
    }
}
