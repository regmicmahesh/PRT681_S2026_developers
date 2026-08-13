namespace JobBoard.Application.Jobs;

public sealed record JobResponse(
    Guid Id,
    string Title,
    string Description,
    string EmploymentType,
    decimal SalaryMin,
    decimal SalaryMax,
    string SalaryCurrency,
    string Status,
    Guid CompanyId,
    int ApplicationCount);
