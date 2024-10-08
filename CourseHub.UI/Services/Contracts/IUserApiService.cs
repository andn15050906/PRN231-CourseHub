﻿using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;

namespace CourseHub.UI.Services.Contracts;

public interface IUserApiService
{
    Task<UserModel?> GetAsync(Guid id);
    Task<UserFullModel?> GetClientAsync(HttpContext context);
    Task<PagedResult<UserModel>> GetPagedAsync(QueryUserDto dto, HttpContext context);
    Task<List<UserOverviewModel>> GetOverviewAsync(IEnumerable<Guid> ids);
    string GetAvatarApiUrl(Guid? id);

    Task<HttpResponseMessage> RegisterAsync(CreateUserDto dto);
    Task<HttpResponseMessage> VerifyEmailAsync(string email, string token);
    Task<HttpResponseMessage> UpdateAsync(UpdateUserDto dto, HttpContext context);
    Task<HttpResponseMessage> RequestPasswordResetAsync(string email);
    Task<HttpResponseMessage> ResetPasswordAsync(ResetPasswordDto dto);

    Task<HttpResponseMessage> SignInAsync(SignInDto dto);
    Task SignOutAsync();
}
