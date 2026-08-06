namespace CleanApp.Application.Common.Interfaces;

/// <summary>Read-only lookup against the user store, for admin views that need to show who owns what
/// or filter by owner without pulling Identity's concrete types into the Application layer.</summary>
public interface IUserDirectory
{
    Task<IReadOnlyDictionary<Guid, string>> GetEmailsByIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Guid>> FindUserIdsByEmailAsync(string emailContains, CancellationToken cancellationToken = default);
}
