using System.Security.Claims;

namespace CourseHub.Core.Interfaces.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(string identifier, string role);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
