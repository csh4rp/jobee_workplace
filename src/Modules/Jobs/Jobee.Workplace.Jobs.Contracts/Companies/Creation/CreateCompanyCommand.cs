using MediatR;
using Jobee.Workplace.Jobs.Contracts.Shared;

namespace Jobee.Workplace.Jobs.Contracts.Companies.Creation;

public class CreateCompanyCommand : IRequest<Guid>
{
    public required string Name { get; init; }

    public required string Description { get; init; }

    public required AddressModel Address { get; init; }
}
