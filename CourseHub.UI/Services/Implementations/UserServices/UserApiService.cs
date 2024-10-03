using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts.UserServices;

namespace CourseHub.UI.Services.Implementations.UserServices;

public class UserApiService : IUserApiService
{
    private readonly HttpClient _client;

    public UserApiService(HttpClient client)
    {
        _client = client;
    }






    public async Task<UserModel?> GetAsync(Guid id)
    {
        try
        {
            var user = await _client.GetFromJsonAsync<UserModel>($"api/users/{id}");
            if (user is null)
                return null;

            user.AvatarUrl = GetAvatarApiUrl(user.AvatarUrl, user.Id);
            return user;
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserFullModel?> GetClientAsync(HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            return await _client.GetFromJsonAsync<UserFullModel>("api/users/client");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<UserOverviewModel>?> GetOverviewAsync(IEnumerable<Guid> ids)
    {
        string url = QueryBuilder.BuildWithArray("api/users/multiple?", "ids={0}&", ids.Select(_ => _.ToString()));

        try
        {
            var result = await _client.GetFromJsonAsync<List<UserOverviewModel>>(url);

            foreach (var item in result)
            {
                item.AvatarUrl = GetAvatarApiUrl(item.AvatarUrl, item.Id);
            }
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<PagedResult<UserModel>> GetPagedAsync(QueryUserDto dto, HttpContext context)
    {
        try
        {
            //_client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<PagedResult<UserModel>>($"api/users?{QueryBuilder.Build(dto)}");
            foreach (var item in result!.Items)
            {
                item.AvatarUrl = GetAvatarApiUrl(item.AvatarUrl, item.Id);
            }
            return result;
        }
        catch
        {
            return PagedResult<UserModel>.GetEmpty();
        }
    }

    public async Task<List<UserMinModel>> GetMinAsync(IEnumerable<Guid> ids)
    {
        string url = QueryBuilder.BuildWithArray("api/users/min?", "ids={0}&", ids.Select(_ => _.ToString()));

        try
        {
            var result = await _client.GetFromJsonAsync<List<UserMinModel>>(url);

            foreach (var item in result)
                item.AvatarUrl = GetAvatarApiUrl(item.AvatarUrl, item.Id);

            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<UserMinModel>> GetAllMinAsync(HttpContext context)
    {
        try
        {
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<List<UserMinModel>>("api/users/all");

            foreach (var item in result)
                item.AvatarUrl = GetAvatarApiUrl(item.AvatarUrl, item.Id);

            return result;
        }
        catch
        {
            return new();
        }
    }

    public static string GetAvatarApiUrl(string avatarUrl, Guid userId)
    {
        if (avatarUrl == string.Empty)
            return "/img/User_Empty.png";
        return ResourceHelper.IsRemote(avatarUrl)
            ? avatarUrl
            : $"{Configurer.GetApiClientOptions().ApiServerPath}/api/users/avatar/{userId}";
    }






    public async Task<HttpResponseMessage> SignInAsync(SignInDto dto)
    {
        return await _client.PostAsJsonAsync("api/auth/SignIn", dto);
    }

    public async Task SignOutAsync()
    {
        await _client.PostAsync("api/auth/SignOut", null);
    }

    public async Task<HttpResponseMessage> RegisterAsync(CreateUserDto dto)
    {
        return await _client.PostAsJsonAsync($"api/users", dto);
    }

    public async Task<HttpResponseMessage> VerifyEmailAsync(string email, string token)
    {
        return await _client.PostAsJsonAsync("api/users/verify", new { email, token });
    }

    public async Task<HttpResponseMessage> UpdateAsync(UpdateUserDto dto, HttpContext context)
    {
        Dictionary<string, string?> kvps = new()
        {
            { nameof(dto.FullName), dto.FullName },
            { nameof(dto.DateOfBirth), dto.DateOfBirth?.ToString() },
            { nameof(dto.Bio), dto.Bio },
            //{ nameof(dto.Phone), dto.Phone },

            { nameof(dto.CurrentPassword), dto.CurrentPassword },
            { nameof(dto.NewPassword), dto.NewPassword }
        };

        MultipartFormDataContent content = new();
        if (dto.Avatar is not null)
        {
            if (dto.Avatar.File is not null)
                content.Add(dto.Avatar.File.AsStreamContent(), "Avatar.File", dto.Avatar.File.FileName);
            else if (dto.Avatar.Url is not null)
                content.Add(new StringContent(dto.Avatar.Url), "Avatar.Url");
        }
        foreach (var pair in kvps)
        {
            if (pair.Value is not null)
                content.Add(new StringContent(pair.Value), pair.Key);
        }

        _client.AddBearerHeader(context);
        var response = await _client.PatchAsync("api/users", content);
        if (response.IsSuccessStatusCode)
        {
            string sResponse = await response.Content.ReadAsStringAsync();
            context.SetClientData(sResponse);
        }
        return response;
    }

    public async Task<HttpResponseMessage> RequestPasswordResetAsync(string email)
    {
        return await _client.PostAsJsonAsync($"api/users/ForgotPassword", email);
    }

    public async Task<HttpResponseMessage> ResetPasswordAsync(ResetPasswordDto dto)
    {
        return await _client.PostAsJsonAsync("api/users/ResetPassword", dto);
    }

    public async Task<HttpResponseMessage> CreateAdminAsync(CreateUserDto dto, HttpContext context)
    {
        _client.AddBearerHeader(context);
        var response = await _client.PostAsJsonAsync($"api/users/admin", dto);
        return response;
    }



    public async Task<HttpResponseMessage> CheckValidityAsync(string email, string token)
    {
        return await _client.GetAsync($"api/users/CheckValidity?email={email}&token={token}");
    }
}
