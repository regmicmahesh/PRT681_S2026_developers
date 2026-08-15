using authentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace authentication.Auth
{
    public static class LoginUser
    {
        public record Request(string Email, string Password);
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (Request request, UserManager<ApplicationUser> userManager, IConfiguration configuration) =>
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Results.Unauthorized();
                }
                var roles = await userManager.GetRolesAsync(user);

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));

                var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims = [
                    new(JwtRegisteredClaimNames.Sub, user.Id),
                    new(JwtRegisteredClaimNames.Email, user.Email!),
                    ..roles.Select(r => new Claim(ClaimTypes.Role, r))
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
                string accessToken = tokenHandler.CreateToken(tokenDescriptor);
                return Results.Ok(new { AccessToken = accessToken, User = user });
            });
        }
    }
}
