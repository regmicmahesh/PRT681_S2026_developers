using System.IdentityModel.Tokens.Jwt;
using CleanApp.Application.Common.Interfaces;
using CleanApp.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace CleanApp.Infrastructure.Identity;

internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public UserId UserId
    {
        get
        {
            var subject = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return subject is not null && Guid.TryParse(subject, out var id) ? new UserId(id) : UserId.Empty;
        }
    }

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
