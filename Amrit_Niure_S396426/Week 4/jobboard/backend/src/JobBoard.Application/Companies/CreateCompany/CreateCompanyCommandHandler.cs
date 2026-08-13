using JobBoard.Application.Messaging;
using JobBoard.Domain.Common;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories;
using JobBoard.Domain.ValueObjects;

namespace JobBoard.Application.Companies.CreateCompany;

internal sealed class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = new Company(Guid.NewGuid(), command.Name, new Email(command.ContactEmail));

        _companyRepository.Add(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return company.Id;
    }
}
