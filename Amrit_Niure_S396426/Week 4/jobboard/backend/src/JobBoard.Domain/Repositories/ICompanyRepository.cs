using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Company company);
}
