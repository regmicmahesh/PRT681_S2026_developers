using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanApp.Infrastructure.Identity;

internal sealed class JwtTokenGenerator(IOptions<JwtSettings> options) : IJwtTokenGenerator
{
    private readonly JwtSettings _settings = options.Value;

    public (string Token, DateTime ExpiresAtUtc) GenerateToken(UserId userId, string email, IReadOnlyCollection<string> roles)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, userId.Value.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SigningKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: signingCredentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }
}
