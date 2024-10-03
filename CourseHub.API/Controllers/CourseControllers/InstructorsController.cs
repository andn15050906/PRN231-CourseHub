using CourseHub.API.Controllers.Shared;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.Core.Entities.CommonDomain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.Services.Domain.CommonServices.Contracts;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using System.Text.Json;

namespace CourseHub.API.Controllers.CourseControllers;

[Produces(contentType: MediaTypeNames.Application.Json)]
public class InstructorsController : BaseController
{
    private readonly IInstructorService _instructorService;

    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }



    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get([FromQuery] QueryInstructorDto dto)
    {
        var result = await _instructorService.GetAsync(dto);
        return result.AsResponse();
    }

    [HttpGet("ByUser/{userId}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var result = await _instructorService.GetByUserIdAsync(userId);
        return result.AsResponse();
    }

    /*[HttpPost]
    [Authorize(Roles = nameof(Role.Learner))]
    public async Task<IActionResult> RequestInstructorRole([FromServices] INotificationService notificationService)
    {
        CreateNotificationDto dto = new() { Type = NotificationType.RequestToBecomeInstructor };
        var client = HttpContext.GetClientId();
        var result = await notificationService.CreateAsync(dto, client);
        return result.AsResponse();
    }*/

    [HttpPost]
    [Authorize(Roles = nameof(Role.Learner))]
    public async Task<IActionResult> RequestInstructorRole(CreateInstructorDto dto, [FromServices] INotificationService notificationService)
    {
        CreateNotificationDto notification = new() {
            Type = NotificationType.RequestToBecomeInstructor,
            Message = JsonSerializer.Serialize(dto)
        };
        var client = HttpContext.GetClientId();
        var result = await notificationService.CreateAsync(notification, client);
        return result.AsResponse();
    }

    [HttpPatch]
    [ResponseCache(Duration = 60)]
    [Authorize]
    public async Task<IActionResult> Update(UpdateInstructorDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _instructorService.UpdateAsync(dto, (Guid)client!);
        return result.AsResponse();
    }
}
