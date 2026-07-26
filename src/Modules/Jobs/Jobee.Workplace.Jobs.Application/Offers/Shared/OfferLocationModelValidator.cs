using FluentValidation;
using Jobee.Workplace.Jobs.Application.Shared;
using Jobee.Workplace.Jobs.Contracts.Offers.Shared;

namespace Jobee.Workplace.Jobs.Application.Offers.Shared;

public class OfferLocationModelValidator : AbstractValidator<OfferLocationModel>
{
    public OfferLocationModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .SetValidator(new AddressModelValidator()!)
            .When(x => x.Address is not null);
    }
}
