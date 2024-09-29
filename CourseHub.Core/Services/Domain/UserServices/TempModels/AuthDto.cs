using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Services.Domain.UserServices.TempModels;

public class AuthDto
{
    public UserFullModel? User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public AuthDto(UserFullModel user, string accessToken, string refreshToken)
    {
        User = user;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
