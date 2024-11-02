using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Services.Domain.CourseServices.Contracts;
using CourseHub.Core.Services.Domain.UserServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers.CourseControllers;

public class EnrollmentsController : BaseController
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> IsEnrolled(Guid courseId)
    {
        Guid client = (Guid)HttpContext.GetClientId()!;
        var result = await _enrollmentService.IsEnrolled(courseId, client);
        return result.AsResponse();
    }

    [HttpGet("ByCourse/{id}")]
    [Authorize]
    public async Task<IActionResult> Get(Guid id)
    {
        Guid client = (Guid)HttpContext.GetClientId()!;
        var result = await _enrollmentService.GetFull(id, client);
        return result.AsResponse();
    }

    [HttpGet("courses")]
    [Authorize]
    public async Task<IActionResult> GetEnrolledCourses()
    {
        var client = (Guid)HttpContext.GetClientId()!;
        var result = await _enrollmentService.Get(client);
        return result.AsResponse();
    }

    //...
    [HttpGet("learners/{courseId}")]
    [Authorize]
    public async Task<IActionResult> GetLearners(Guid courseId)
    {
        var client = (Guid)HttpContext.GetClientId()!;
        var result = await _enrollmentService.GetLearners(courseId, client);
        return result.AsResponse();
    }

    [HttpPost("GrantEnrollment")]
    [Authorize(Roles = RoleConstants.SYSADMIN)]
    public async Task<IActionResult> GrantEnrollment(Guid courseId, Guid userId, [FromServices] IUserService userService)
    {
        var user = await userService.GetAsync(userId);
        var result = await _enrollmentService.GrantEnrollment(courseId, userId);
        return result.AsResponse();
    }
}
