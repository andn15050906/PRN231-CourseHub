using CourseHub.Core.Models.User.UserModels;

namespace CourseHub.Core.Services.Domain.UserServices.TempModels;

public class AuthModel
{
    public UserFullModel? User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public AuthModel(UserFullModel user, string accessToken, string refreshToken)
    {
        User = user;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
