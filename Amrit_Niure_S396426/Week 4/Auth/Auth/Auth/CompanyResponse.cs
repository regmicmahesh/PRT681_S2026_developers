using Auth.Data;

namespace Auth.Auth;

public record CompanyResponse(Guid Id, string Name, string ContactEmail, string OwnerId, DateTime CreatedAtUtc)
{
    public static CompanyResponse FromCompany(Company company) =>
        new(company.Id, company.Name, company.ContactEmail, company.OwnerId, company.CreatedAtUtc);
}
