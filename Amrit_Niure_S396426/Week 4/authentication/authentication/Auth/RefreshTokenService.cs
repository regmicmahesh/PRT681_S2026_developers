using authentication.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace authentication.Auth;

public sealed record RefreshRotationResult(bool Success, string? UserId, string? NewRawToken);

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

    public async Task<RefreshRotationResult> ValidateAndRotateAsync(string rawToken, CancellationToken ct = default)
    {
        var hash = Hash(rawToken);
        var existing = await dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == hash, ct);

        if (existing is null)
        {
            return new RefreshRotationResult(false, null, null);
        }

        if (!existing.IsActive)
        {
            // Reuse of a revoked/expired token is a signal of possible theft — revoke everything for this user.
            if (existing.RevokedAtUtc is not null)
            {
                await RevokeAllForUserAsync(existing.UserId, ct);
            }

            return new RefreshRotationResult(false, null, null);
        }

        existing.RevokedAtUtc = DateTime.UtcNow;
        var (newEntity, newRawToken) = await IssueAsync(existing.UserId, ct);
        existing.ReplacedByTokenHash = newEntity.TokenHash;
        await dbContext.SaveChangesAsync(ct);

        return new RefreshRotationResult(true, existing.UserId, newRawToken);
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

    public async Task RevokeAllForUserAsync(string userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        await dbContext.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAtUtc == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.RevokedAtUtc, now), ct);
    }
}
