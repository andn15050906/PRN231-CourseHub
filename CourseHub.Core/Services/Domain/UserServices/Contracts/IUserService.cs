using CourseHub.Core.Entities.UserDomain.Enums;
using CourseHub.Core.Helpers.Messaging;
using CourseHub.Core.Interfaces.Authentication;
using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.Core.Services.Domain.UserServices.TempModels;
using System.Security.Claims;

namespace CourseHub.Core.Services.Domain.UserServices.Contracts;

public interface IUserService
{
    Task<ServiceResult<UserFullModel>> GetFullAsync(Guid id);
    Task<ServiceResult<UserModel>> GetAsync(Guid id);
    Task<ServiceResult<PagedResult<UserModel>>> GetPagedAsync(QueryUserDto dto);
    Task<ServiceResult<List<UserOverviewModel>>> GetOverviewAsync(List<Guid> ids);
    Task<ServiceResult<List<UserMinModel>>> GetMinAsync(List<Guid> ids);

    Task<ServiceResult<string>> CreateAsync(CreateUserDto dto);
    Task<ServiceResult<string>> CreateAdminAsync(CreateUserDto dto);
    Task<ServiceResult> VerifyAsync(VerifyEmailDto dto);
    Task<ServiceResult<UserFullModel>> UpdateAsync(UpdateUserDto dto, Guid? clientId);
    Task<ServiceResult<string>> GetPasswordResetTokenAsync(string email);
    Task<ServiceResult> ResetPasswordAsync(ResetPasswordDto dto);
    Task<ServiceResult> BlockAsync(Guid userId);

    Task<ServiceResult<AuthModel>> SignInAsync(SignInDto dto, ITokenService tokenService);
    Task<ServiceResult<ClaimAuthModel>> ExternalSignInAsync(ClaimsPrincipal claimsPrincipal, Role role);
    Task<ServiceResult<AuthModel>> RefreshAsync(string? accessToken, string? refreshToken, ITokenService tokenService);

    Task<ServiceResult> IsValidToken(string email, string token);
    Task ForceCommitAsync();
    Task<ServiceResult<List<UserMinModel>>> GetAllMinAsync();
}
