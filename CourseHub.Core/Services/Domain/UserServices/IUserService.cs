using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Authentication;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.Core.Services.Domain.UserServices.TempModels;
using System.Security.Claims;

namespace CourseHub.Core.Services.Domain.UserServices;

public interface IUserService
{
    Task<ServiceResult<UserFullModel>> GetFullAsync(Guid id);
    Task<ServiceResult<UserModel>> GetAsync(Guid id);
    Task<ServiceResult<PagedResult<UserModel>>> GetPagedAsync(QueryUserDto dto);
    Task<ServiceResult<List<UserOverviewModel>>> GetOverviewAsync(List<Guid> ids);

    Task<ServiceResult<string>> CreateAsync(CreateUserDto dto);
    Task<ServiceResult> VerifyAsync(VerifyEmailDto dto);
    Task<ServiceResult<UserFullModel>> UpdateAsync(UpdateUserDto dto, Guid? userId);
    Task<ServiceResult<string>> GetPasswordResetTokenAsync(string email);
    Task<ServiceResult> ResetPasswordAsync(ResetPasswordDto dto);

    Task<ServiceResult<AuthDto>> SignInAsync(SignInDto dto, ITokenService tokenService);
    Task<ServiceResult<ClaimsPrincipal>> ExternalSignInAsync(ClaimsPrincipal claimsPrincipal, Role role);
    Task<ServiceResult<AuthDto>> RefreshAsync(string? accessToken, string? refreshToken, ITokenService tokenService);
}
