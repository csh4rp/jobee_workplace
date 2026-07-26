using FluentValidation;
using Jobee.Workplace.Jobs.Contracts.Offers.Shared;

namespace Jobee.Workplace.Jobs.Application.Offers.Shared;

public class OfferRequirementModelValidator : AbstractValidator<OfferRequirementModel>
{
    public OfferRequirementModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Level)
            .IsInEnum();
    }
}
