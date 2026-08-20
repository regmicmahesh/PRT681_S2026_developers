using ExpenseTrackerAuth.Dtos;
using ExpenseTrackerAuth.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerAuth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _users;
        private readonly TokenService _tokens;

        public AuthController(UserManager<IdentityUser> users, TokenService tokens)
        {
            _users = users;
            _tokens = tokens;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _users.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            var (token, expiresAt) = _tokens.CreateToken(user);
            return Ok(new AuthResponseDto(token, user.Email!, expiresAt));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _users.FindByEmailAsync(dto.Email);
            if (user is null || !await _users.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            var (token, expiresAt) = _tokens.CreateToken(user);
            return Ok(new AuthResponseDto(token, user.Email!, expiresAt));
        }
    }
}
