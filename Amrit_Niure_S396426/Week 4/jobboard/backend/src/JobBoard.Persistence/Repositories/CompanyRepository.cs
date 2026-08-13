using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Persistence.Repositories;

internal sealed class CompanyRepository : ICompanyRepository
{
    private readonly JobBoardDbContext _dbContext;

    public CompanyRepository(JobBoardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public void Add(Company company) => _dbContext.Companies.Add(company);
}
