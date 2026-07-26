using FluentValidation;
using Jobee.Workplace.Jobs.Application.Shared;
using Jobee.Workplace.Jobs.Contracts.Companies.Updating;

namespace Jobee.Workplace.Jobs.Application.Companies.Updating;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    { 
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.Address)
            .NotNull()
            .SetValidator(new AddressModelValidator());
    }
}
