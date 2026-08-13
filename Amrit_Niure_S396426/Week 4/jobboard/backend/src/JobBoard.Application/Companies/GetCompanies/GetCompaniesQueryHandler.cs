using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Repositories;

namespace JobBoard.Application.Companies.GetCompanies;

internal sealed class GetCompaniesQueryHandler : IQueryHandler<GetCompaniesQuery, IReadOnlyList<CompanyResponse>>
{
    private readonly ICompanyRepository _companyRepository;

    public GetCompaniesQueryHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Result<IReadOnlyList<CompanyResponse>>> Handle(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllAsync(cancellationToken);

        IReadOnlyList<CompanyResponse> response = companies
            .Select(company => new CompanyResponse(company.Id, company.Name, company.ContactEmail.Value))
            .ToList();

        return Result.Success(response);
    }
}
