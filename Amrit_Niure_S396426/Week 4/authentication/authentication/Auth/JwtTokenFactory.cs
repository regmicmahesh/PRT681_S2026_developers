using authentication.Authorization;
using authentication.Data;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace authentication.Auth;

public static class JwtTokenFactory
{
    public static string CreateAccessToken(
        ApplicationUser user,
        IEnumerable<string> roles,
        IEnumerable<string> permissions,
        IConfiguration configuration)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            ..roles.Select(r => new Claim(ClaimTypes.Role, r)),
            ..permissions.Select(p => new Claim(CustomClaimTypes.Permission, p))
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
        };

        var tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(tokenDescriptor);
    }
}
