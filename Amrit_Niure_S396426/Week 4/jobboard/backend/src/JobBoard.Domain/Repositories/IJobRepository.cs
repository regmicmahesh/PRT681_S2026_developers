using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Repositories;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Job>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Job job);

    // EF Core can't distinguish a brand-new child entity (app-assigned Guid key) from an
    // existing one when it's only discovered via the Job.Applications navigation, so newly
    // created applications must be tracked explicitly instead of relying on change detection.
    void AddApplication(JobApplication application);
}
