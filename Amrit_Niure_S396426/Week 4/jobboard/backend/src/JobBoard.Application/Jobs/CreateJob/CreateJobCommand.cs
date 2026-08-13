using JobBoard.Application.Messaging;
using JobBoard.Domain.Enums;

namespace JobBoard.Application.Jobs.CreateJob;

public sealed record CreateJobCommand(
    string Title,
    string Description,
    EmploymentType EmploymentType,
    decimal SalaryMin,
    decimal SalaryMax,
    string SalaryCurrency,
    PayPeriod PayPeriod,
    Guid CompanyId) : ICommand<Guid>;
