using CourseHub.API.Controllers.Shared;
using CourseHub.API.Helpers.Cookie;
using CourseHub.API.Services.AppInfo;
using CourseHub.API.Services.Email;
using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Logging;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.Core.Services.Domain.UserServices.Contracts;
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
    public async Task<IActionResult> GetAsync(Guid id)
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
    public async Task<IActionResult> GetAsync([FromQuery] QueryUserDto dto)
    {
        ServiceResult<PagedResult<UserModel>> result = await _userService.GetPagedAsync(dto);
        return result.AsResponse();
    }

    [HttpGet("multiple")]
    public async Task<IActionResult> GetMultipleAsync([FromQuery] List<Guid> ids)
    {
        ServiceResult<List<UserOverviewModel>> result = await _userService.GetOverviewAsync(ids);
        return result.AsResponse();
    }

    [HttpGet("min")]
    public async Task<IActionResult> GetMinAsync([FromQuery] List<Guid> ids)
    {
        ServiceResult<List<UserMinModel>> result = await _userService.GetMinAsync(ids);
        return result.AsResponse();
    }

    //...
    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllMinAsync()
    {
        ServiceResult<List<UserMinModel>> result = await _userService.GetAllMinAsync();
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

        if (result.IsSuccessful)
        {
            string link = $"{appInfo.Value.MainFrontendApp}/verify-email/{dto.Email}/{result.Data}";
#pragma warning disable CS4014
            await emailService.SendRegistrationEmail(dto.Email, dto.UserName, link);
#pragma warning restore CS4014
        }

        return result.AsResponse();
    }

    [HttpPost("admin")]
    [Authorize(Roles = nameof(Role.SysAdmin))]
    public async Task<IActionResult> CreateAdminAsync(CreateUserDto dto)
    {
        var result = await _userService.CreateAdminAsync(dto);
        return result.AsResponse();
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
    {
        var result = await _userService.VerifyAsync(dto);
        return result.AsResponse();
    }

    [HttpPost("block/{id}")]
    [Authorize(Roles = RoleConstants.ADMIN_OR_SYSADMIN)]
    public async Task<IActionResult> Block(Guid id)
    {
        ServiceResult result = await _userService.BlockAsync(id);
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

    [HttpGet("CheckValidity")]
    public async Task<IActionResult> CheckValidity(string email, string token)
    {
        var result = await _userService.IsValidToken(email, token);
        return result.AsResponse();
    }
}
