using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Assignment.AssignmentDtos;
using CourseHub.Core.Services.Domain.AssignmentServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CourseHub.API.Controllers.AssignmentControllers;

public class AssignmentsController : BaseController
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }



    [HttpGet("{id}")]
    [Authorize]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _assignmentService.GetAsync(id);
        return result.AsResponse();
    }

    [HttpGet("{id}/min")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetMin(Guid id)
    {
        var result = await _assignmentService.GetMinAsync(id);
        return result.AsResponse();
    }

    [HttpGet("BySections")]
    public async Task<IActionResult> GetBySections([FromQuery] List<Guid> sections)
    {
        var result = await _assignmentService.GetBySectionsAsync(sections);
        return result.AsResponse();
    }

    [HttpGet("ByCourse")]
    public async Task<IActionResult> GetByCourse([FromQuery] Guid courseId)
    {
        var result = await _assignmentService.GetByCourseAsync(courseId);
        return result.AsResponse();
    }






    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateAssignmentDto dto)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _assignmentService.CreateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateAssignmentDto dto)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _assignmentService.UpdateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var clientId = (Guid)HttpContext.GetClientId()!;
        var result = await _assignmentService.DeleteAsync(id, clientId);
        return result.AsResponse();
    }
}
