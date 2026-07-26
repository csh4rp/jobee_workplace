using Jobee.Workplace.Shared.Domain;

namespace Jobee.Workplace.Jobs.Domain.Companies;

public record CompanyCreated(Guid CompanyId) : IDomainEvent;
