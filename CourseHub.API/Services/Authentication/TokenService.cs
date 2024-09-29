using CourseHub.Core.Interfaces.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CourseHub.API.Services.Authentication;

public class TokenService : ITokenService
{
    private readonly TokenOptions _options;

    public TokenService(IOptions<TokenOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateAccessToken(string identifier, string role)
    {
        //generating new Guid each time called
        List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, identifier),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(_options.Secret));
        JwtSecurityToken token = new(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: DateTime.UtcNow.AddSeconds(_options.Lifetime),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        TokenValidationParameters parameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
            ValidAudience = _options.Audience,
            ValidIssuer = _options.Issuer,
        };

        ClaimsPrincipal principal = new JwtSecurityTokenHandler()
            .ValidateToken(token, parameters, out SecurityToken securityToken);

        //Check if accessToken is invalid
        if (securityToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            return null;

        //Check if accessToken is expired (do not allow refreshing before expiration)
        string expire = principal.Claims.FirstOrDefault(_ => _.Type == JwtRegisteredClaimNames.Exp)!.Value;
        DateTime expireDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expire)).UtcDateTime;
        if (DateTime.UtcNow < expireDate)
            return null;

        return principal;
    }
}
