using JobBoard.Application.Messaging;

namespace JobBoard.Application.Companies.CreateCompany;

public sealed record CreateCompanyCommand(string Name, string ContactEmail) : ICommand<Guid>;
