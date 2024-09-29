using CourseHub.Core.Models.User.UserModels;
using System.Security.Claims;

namespace CourseHub.Core.Services.Domain.UserServices.TempModels;

public class ClaimAuthModel
{
    public const string HTTP_HEADER = "X-Custom-UserInfo";

    public UserFullModel? User { get; set; }
    public ClaimsPrincipal Principal { get; set; }

    public ClaimAuthModel(UserFullModel user, ClaimsPrincipal principal)
    {
        User = user;
        Principal = principal;
    }
}
