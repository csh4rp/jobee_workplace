using MediatR;
using Jobee.Workplace.Jobs.Contracts.Shared;

namespace Jobee.Workplace.Jobs.Contracts.Companies.Updating;

public class UpdateCompanyCommand : IRequest
{
    public Guid Id { get; private set; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required AddressModel Address { get; init; }

    public void SetId(Guid id) => Id = id;
}

