using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Net.Mime;
using CourseHub.Core.Interfaces.Authentication;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.API.Helpers.Cookie;
using CourseHub.API.Controllers.Shared;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.Core.Services.Domain.UserServices.TempModels;
using CourseHub.Core.Services.Domain.UserServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using CourseHub.API.Services.AppInfo;
using Microsoft.Extensions.Options;
using CourseHub.Core.Entities.UserDomain.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CourseHub.API.Controllers.UserControllers;

[Consumes(contentType: MediaTypeNames.Application.Json)]
[Produces(contentType: MediaTypeNames.Application.Json)]
public class AuthController : BaseController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }






    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto dto, [FromServices] ITokenService tokenService)
    {
        ServiceResult<AuthDto> result = await _userService.SignInAsync(dto, tokenService);

        if (result.IsSuccessful)
            SetAuthData(result.Data!);
        return result.AsResponse();
    }

    [HttpGet("google-oauth/{role}")]
    public IActionResult SignInWithGoogle(Role role)
    {
        return Challenge(
            new AuthenticationProperties() { RedirectUri = $"/api/auth/oauth-callback/{(byte)role}" },
            GoogleDefaults.AuthenticationScheme
        );
    }

    [HttpGet("oauth-callback/{role}")]
    public async Task<IActionResult> OAuthCallback(Role role, [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        // Make database and claims be in a valid state
        var principalResult = await _userService.ExternalSignInAsync(User, role);
        if (principalResult.Data is null)
            return principalResult.AsResponse();
        await HttpContext.SignInAsync(principalResult.Data);

        // After this, there will be two NameIdentifier Claims
        return Redirect($"{appInfo.Value.MainFrontendApp}/external");
    }

    [HttpPost("SignOut")]
    public new void SignOut()
    {
        Response.SetCredentials("", "", CookieHelper.GetExpiredOptions());
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh([FromServices] ITokenService tokenService)
    {
        ServiceResult<AuthDto> result = await _userService.RefreshAsync(
            Request.GetAccessToken(),
            Request.GetRefreshToken(),
            tokenService
        );

        if (result.IsSuccessful)
            SetAuthData(result.Data!);
        return result.AsResponse();
    }



    private void SetAuthData(AuthDto authData)
    {
        Response.SetCredentials(authData.AccessToken, authData.RefreshToken, CookieHelper.GetOptions());
    }
}
