using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Companies;

public record CompanyUpdated(Guid CompanyId) : IDomainEvent;