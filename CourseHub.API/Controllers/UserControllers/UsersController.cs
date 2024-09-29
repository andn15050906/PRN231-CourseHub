using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.API.Services.AppInfo;
using CourseHub.API.Services.Email;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.Core.Services.Domain.UserServices;
using CourseHub.Core.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CourseHub.API.Controllers.UserControllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;
    private readonly IAppLogger _logger;

    public UsersController(IUserService userService, IAppLogger logger)
    {
        _userService = userService;
        _logger = logger;
    }






    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfoAsync(Guid id)
    {
        ServiceResult<UserModel> result = await _userService.GetAsync(id);
        return result.AsResponse();
    }

    [HttpGet("client")]
    [Authorize]
    public async Task<IActionResult> GetClientInfoAsync()
    {
        var clientId = (Guid)this.HttpContext.GetClientId()!;
        ServiceResult<UserFullModel> result = await _userService.GetFullAsync(clientId);
        return result.AsResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersAsync([FromQuery] QueryUserDto dto)
    {
        ServiceResult<PagedResult<UserModel>> result = await _userService.GetPagedAsync(dto);
        return result.AsResponse();
    }

    [HttpGet("multiple")]
    public async Task<IActionResult> GetMultipleUsersAsync([FromQuery] List<Guid> ids)
    {
        ServiceResult<List<UserOverviewModel>> result = await _userService.GetOverviewAsync(ids);
        return result.AsResponse();
    }

    [HttpGet("avatar/{resourceId}")]
    public IActionResult GetAvatar(Guid resourceId)
    {
        // if null, find the next resource related to user
        Stream? stream = ServerStorage.ReadAsStream(UserStorage.GetAvatarPath(resourceId));
        return stream is null ? NotFound() : new FileStreamResult(stream, "image/jpeg");
    }






    [HttpPost]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] CreateUserDto dto,
        [FromServices] EmailService emailService, [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        ServiceResult<string> result = await _userService.CreateAsync(dto);

        string link = $"{appInfo.Value.MainFrontendApp}/verify-email/{dto.Email}/{result.Data}";
#pragma warning disable CS4014
        emailService.SendRegistrationEmail(dto.Email, dto.UserName, link);
#pragma warning restore CS4014

        return result.AsResponse();
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
    {
        var result = await _userService.VerifyAsync(dto);
        return result.AsResponse();
    }



    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateAsync([FromForm] UpdateUserDto dto)
    {
        ServiceResult<UserFullModel> result = await _userService.UpdateAsync(dto, HttpContext.GetClientId());
        return result.AsResponse();
    }



    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> RequestPasswordResetAsync(
        [FromBody] string email,
        [FromServices] EmailService emailService, [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        ServiceResult<string> result = await _userService.GetPasswordResetTokenAsync(email);

        if (!result.IsSuccessful)
            return result.AsResponse();

        string link = $"{appInfo.Value.MainFrontendApp}/reset-password/{email}/{result.Data}";
#pragma warning disable CS4014
        emailService.SendPasswordResetEmail(email, link);
#pragma warning restore CS4014

        return Ok();
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var result = await _userService.ResetPasswordAsync(dto);
        return result.AsResponse();
    }
}
