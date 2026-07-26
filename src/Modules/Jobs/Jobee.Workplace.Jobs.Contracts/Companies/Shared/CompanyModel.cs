using Jobee.Workplace.Jobs.Contracts.Shared;

namespace Jobee.Workplace.Jobs.Contracts.Companies.Shared;

public record CompanyModel
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required AddressModel Address { get; init; }
}

