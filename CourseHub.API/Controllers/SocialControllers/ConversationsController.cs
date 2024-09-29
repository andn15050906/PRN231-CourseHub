using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.RequestDtos.Social.ConversationDtos;
using CourseHub.Core.Services.Domain.SocialServices.Contracts;
using CourseHub.Core.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers.SocialControllers;

public class ConversationsController : BaseController
{
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }



    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryConversationDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _conversationService.Get(dto, client);
        return result.AsResponse();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var client = HttpContext.GetClientId();
        var result = await _conversationService.Get(id, client);
        return result.AsResponse();
    }

    [HttpGet("multiple")]
    public async Task<IActionResult> GetMultiple([FromQuery] List<Guid> ids)
    {
        var result = await _conversationService.GetMultiple(ids);
        return result.AsResponse();
    }

    [HttpGet("targets")]
    public async Task<IActionResult> GetConversationTargets([FromQuery] QueryConversationDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _conversationService.GetConversationsOrUsers(dto, client);
        return result.AsResponse();
    }

    /*[HttpGet("{id}/outsiders")]
    [Authorize]
    public async Task<IActionResult> GetConversationOutsiders(Guid id)
    {
        var client = HttpContext.GetClientId();
        var result = await _conversationService.GetOutsiders(dto, client);
        return result.AsResponse();
    }*/

    [HttpGet("Resource/{resourceId}")]
    public IActionResult GetAvatar(Guid resourceId)
    {
        Stream? stream = ServerStorage.ReadAsStream(SocialStorage.GetAvatarPath(resourceId));
        return stream is null ? NotFound() : new FileStreamResult(stream, "image/jpeg");
    }



    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateConversation([FromForm] CreateConversationDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _conversationService.Create(dto, (Guid)client!);
        return result.AsResponse();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateConversations([FromForm] UpdateConversationDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _conversationService.Update(dto, (Guid)client!);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteConversation(Guid id)
    {
        var client = HttpContext.GetClientId();
        var result = await _conversationService.Delete(id, (Guid)client!);
        return result.AsResponse();
    }
}
