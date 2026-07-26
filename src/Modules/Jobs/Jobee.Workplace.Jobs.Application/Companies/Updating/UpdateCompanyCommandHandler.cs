using Jobee.Workplace.Jobs.Application.Offers;
using Jobee.Workplace.Jobs.Contracts.Companies.Updating;
using Jobee.Workplace.Jobs.Domain.Companies;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Jobs.Application.Companies.Updating;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ILogger<UpdateCompanyCommandHandler> _logger;

    public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, ILogger<UpdateCompanyCommandHandler> logger)
    {
        _companyRepository = companyRepository;
        _logger = logger;
    }

    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id, cancellationToken);

        company.Update(
            request.Name,
            request.Description,
            request.Address.ToEntity());

        await _companyRepository.UpdateAsync(company, cancellationToken);

        _logger.LogInformation("Company {CompanyId} updated", company.Id);
    }
}

