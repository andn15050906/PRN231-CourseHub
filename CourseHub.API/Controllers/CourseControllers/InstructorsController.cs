using CourseHub.API.Controllers.Shared;
using CourseHub.Core.RequestDtos.Course.InstructorDtos;
using CourseHub.Core.Services.Domain.CourseServices;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

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
    public async Task<IActionResult> Get(QueryInstructorDto dto)
    {
        var result = await _instructorService.GetAsync(dto);
        return result.AsResponse();
    }

    [HttpPost]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Update(UpdateInstructorDto dto)
    {
        var result = await _instructorService.UpdateAsync(dto);
        return result.AsResponse();
    }
}
