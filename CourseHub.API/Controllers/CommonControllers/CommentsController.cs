using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Common.CommentDtos;
using CourseHub.Core.Services.Domain.CommonServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CourseHub.API.Controllers.CommonControllers;

public class CommentsController : BaseController
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }



    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get([FromQuery] QueryCommentDto dto)
    {
        var result = await _commentService.GetPagedAsync(dto);
        return result.AsResponse();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCommentDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _commentService.CreateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateCommentDto dto)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _commentService.UpdateAsync(dto, clientId);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var clientId = HttpContext.GetClientId();
        var result = await _commentService.DeleteAsync(id, clientId);
        return result.AsResponse();
    }
}
