using Domain.Entities;

namespace Domain.Repositories;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobApplication>> GetByJobPostIdAsync(Guid jobPostId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobApplication>> GetByJobSeekerIdAsync(Guid jobSeekerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid jobPostId, Guid jobSeekerId, CancellationToken cancellationToken = default);
    Task AddAsync(JobApplication application, CancellationToken cancellationToken = default);
    void Update(JobApplication application);
}
