using authentication.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace authentication.Auth;

public sealed record RefreshValidationResult(bool Success, string? UserId);

public sealed class RefreshTokenService(ApplicationDbContext dbContext, IConfiguration configuration)
{
    public static string GenerateRawToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public static string Hash(string rawToken) =>
        Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

    public async Task<(RefreshToken Entity, string RawToken)> IssueAsync(string userId, CancellationToken ct = default)
    {
        var rawToken = GenerateRawToken();
        var entity = new RefreshToken
        {
            UserId = userId,
            TokenHash = Hash(rawToken),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(configuration.GetValue("Jwt:RefreshTokenExpirationInDays", 7)),
        };

        dbContext.RefreshTokens.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        return (entity, rawToken);
    }

    // No rotation: the same refresh token remains valid (and reusable to mint new access tokens)
    // until it naturally expires or is revoked via /logout. Simpler client contract, less server
    // state to manage - add rotation/reuse-detection later if there's an actual need to defend
    // against refresh-token theft.
    public async Task<RefreshValidationResult> ValidateAsync(string rawToken, CancellationToken ct = default)
    {
        var hash = Hash(rawToken);
        var existing = await dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == hash, ct);

        return existing is not null && existing.IsActive
            ? new RefreshValidationResult(true, existing.UserId)
            : new RefreshValidationResult(false, null);
    }

    public async Task RevokeAsync(string rawToken, CancellationToken ct = default)
    {
        var hash = Hash(rawToken);
        var existing = await dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == hash, ct);
        if (existing is not null && existing.RevokedAtUtc is null)
        {
            existing.RevokedAtUtc = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(ct);
        }
    }
}
