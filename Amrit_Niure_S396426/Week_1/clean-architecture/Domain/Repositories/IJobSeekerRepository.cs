using Domain.Entities;

namespace Domain.Repositories;

public interface IJobSeekerRepository
{
    Task<JobSeeker?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<JobSeeker?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(JobSeeker jobSeeker, CancellationToken cancellationToken = default);
    void Update(JobSeeker jobSeeker);
    void Delete(JobSeeker jobSeeker);
}
