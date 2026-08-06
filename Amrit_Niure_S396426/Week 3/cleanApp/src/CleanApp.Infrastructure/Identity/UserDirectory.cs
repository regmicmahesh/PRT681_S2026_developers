using CleanApp.Application.Common.Interfaces;
using CleanApp.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Infrastructure.Identity;

internal sealed class UserDirectory(UserManager<ApplicationUser> userManager) : IUserDirectory
{
    public async Task<IReadOnlyDictionary<Guid, string>> GetEmailsByIdsAsync(
        IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var idSet = userIds.ToHashSet();

        return await userManager.Users
            .Where(u => idSet.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.Email ?? string.Empty, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Guid>> FindUserIdsByEmailAsync(
        string emailContains, CancellationToken cancellationToken = default) =>
        await userManager.Users
            .Where(u => u.Email != null && u.Email.Contains(emailContains))
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);
}
