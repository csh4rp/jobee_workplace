using FluentValidation;
using Jobee.Workplace.Jobs.Application.Shared;
using Jobee.Workplace.Jobs.Contracts.Companies.Creation;

namespace Jobee.Workplace.Jobs.Application.Companies.Creation;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
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
