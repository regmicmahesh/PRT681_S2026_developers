using CleanApp.Domain.Common;

namespace CleanApp.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(UserId userId, string email, IReadOnlyCollection<string> roles);
}
