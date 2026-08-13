using JobBoard.Application.Messaging;

namespace JobBoard.Application.Companies.GetCompanies;

public sealed record GetCompaniesQuery : IQuery<IReadOnlyList<CompanyResponse>>;
