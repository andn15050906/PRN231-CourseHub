using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Assignment.SubmissionDtos;
using CourseHub.Core.Services.Domain.AssignmentServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers.AssignmentControllers;

public class SubmissionsController : BaseController
{
    private readonly ISubmissionService _submissionService;

    public SubmissionsController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }



    [HttpGet("{id}")]
    [Authorize]
    //[ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _submissionService.GetAsync(id);
        return result.AsResponse();
    }

    [HttpGet("ByAssignment")]
    [Authorize]
    //[ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetByAssignmentId([FromQuery] Guid assignmentId)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _submissionService.GetByAssignmentId(assignmentId, clientId);
        return result.AsResponse();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateSubmissionDto dto)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _submissionService.CreateAsync(dto, clientId);
        return result.AsResponse();
    }
}
