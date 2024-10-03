using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Common.NotificationDtos;
using CourseHub.Core.Services.Domain.CommonServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers.CommonControllers;

public class NotificationsController : BaseController
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }



    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryNotificationDto dto)
    {
        var result = await _notificationService.GetPagedAsync(dto);
        return result.AsResponse();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateNotificationDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _notificationService.CreateAsync(dto, client);
        return result.AsResponse();
    }

    [HttpPost("multiple")]
    [Authorize]
    public async Task<IActionResult> CreateMultiple(CreateMultipleNotificationDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _notificationService.CreateAsync(dto, client);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateNotificationDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _notificationService.UpdateAsync(dto, client);
        return result.AsResponse();
    }
}
