namespace CleanApp.Application.Auth;

public sealed record AuthResponse(Guid UserId, string Email, string Token, DateTime ExpiresAtUtc);
