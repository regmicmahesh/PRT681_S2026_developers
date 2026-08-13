using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Persistence.Repositories;

internal sealed class JobRepository : IJobRepository
{
    private readonly JobBoardDbContext _dbContext;

    public JobRepository(JobBoardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbContext.Jobs
            .Include(j => j.Applications)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Job>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.Jobs
            .Include(j => j.Applications)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public void Add(Job job) => _dbContext.Jobs.Add(job);

    public void AddApplication(JobApplication application) => _dbContext.Set<JobApplication>().Add(application);
}
