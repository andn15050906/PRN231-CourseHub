using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.Core.RequestDtos.Social.ChatMessageDtos;
using CourseHub.Core.Services.Domain.SocialServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers.SocialControllers;

public class ChatMessagesController : BaseController
{
    private readonly IChatMessageService _chatMessageService;

    public ChatMessagesController(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetMessages([FromQuery] QueryChatMessageDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _chatMessageService.Get(dto, (Guid)client!);
        return result.AsResponse();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateMessage(CreateChatMessageDto dto)
    {
        var client = HttpContext.GetClientId();
        var result = await _chatMessageService.Create(dto, (Guid)client!);
        return result.AsResponse();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        var client = HttpContext.GetClientId();
        var result = await _chatMessageService.Delete(id, (Guid)client!);
        return result.AsResponse();
    }
}
