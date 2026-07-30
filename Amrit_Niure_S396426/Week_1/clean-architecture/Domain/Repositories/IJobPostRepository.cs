using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories;

public interface IJobPostRepository
{
    Task<JobPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobPost>> GetActiveJobPostsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobPost>> GetByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobPost>> SearchAsync(
        string? searchTerm, 
        Guid? categoryId, 
        JobType? jobType, 
        WorkMode? workMode, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default);
    Task AddAsync(JobPost jobPost, CancellationToken cancellationToken = default);
    void Update(JobPost jobPost);
    void Delete(JobPost jobPost);
}
