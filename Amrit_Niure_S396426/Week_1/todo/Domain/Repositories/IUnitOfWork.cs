namespace Domain.Repositories;

/// <summary>
/// Commits everything changed on tracked aggregates in a single transaction, then
/// dispatches any domain events those aggregates raised. Application handlers depend on
/// this instead of talking to EF Core directly, so the Application layer stays persistence-agnostic.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
