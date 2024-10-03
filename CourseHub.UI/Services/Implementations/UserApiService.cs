using CourseHub.Core.Interfaces.Repositories.Shared;
using CourseHub.Core.Models.User.UserModels;
using CourseHub.Core.RequestDtos.User.UserDtos;
using CourseHub.UI.Helpers.Http;
using CourseHub.UI.Helpers.Utils;
using CourseHub.UI.Services.Contracts;

namespace CourseHub.UI.Services.Implementations;

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
            return await _client.GetFromJsonAsync<UserModel>($"api/users/{id}");
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
            return await _client.GetFromJsonAsync<List<UserOverviewModel>>(url);
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
            _client.AddBearerHeader(context);
            var result = await _client.GetFromJsonAsync<PagedResult<UserModel>>($"api/users");
            return result!;
        }
        catch
        {
            return PagedResult<UserModel>.GetEmpty();
        }
    }

    public string GetAvatarApiUrl(Guid? id)
    {
        if (id is null)
            return "/image/user/User_Empty.png";
        return $"{_client.BaseAddress}api/users/avatar/{id}";
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
                content.Add(dto.Avatar.File.AsStreamContent(), nameof(dto.Avatar.File), dto.Avatar.File.FileName);
            else if (dto.Avatar.Url is not null)
                content.Add(new StringContent(dto.Avatar.Url), nameof(dto.Avatar.Url));
        }
        foreach (var pair in kvps)
        {
            if (pair.Value is not null)
                content.Add(new StringContent(pair.Value), pair.Key);
        }

        _client.AddBearerHeader(context);
        var response = await _client.PatchAsync("api/users", content);
        if (response.IsSuccessStatusCode)
            context.SetClientData(await response.Content.ReadAsStringAsync());
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
}
