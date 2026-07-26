using Jobee.Workplace.Jobs.Application.Offers;
using Jobee.Workplace.Jobs.Contracts.Companies.Creation;
using Jobee.Workplace.Jobs.Domain.Companies;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Companies.Creation;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ILogger<CreateCompanyCommandHandler> _logger;

    public CreateCompanyCommandHandler(ICompanyRepository companyRepository, ILogger<CreateCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company(
            request.Name,
            request.Description,
            request.Address.ToEntity());

        await _companyRepository.AddAsync(company, cancellationToken);

        _logger.LogInformation("Company {CompanyId} created", company.Id);

        return company.Id;
    }
}

