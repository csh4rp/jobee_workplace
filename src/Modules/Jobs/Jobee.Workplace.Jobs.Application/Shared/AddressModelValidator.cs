using FluentValidation;
using Jobee.Workplace.Jobs.Contracts.Shared;

namespace Jobee.Workplace.Jobs.Application.Shared;

public class AddressModelValidator : AbstractValidator<AddressModel>
{
    public AddressModelValidator()
    {
        RuleFor(x => x.City)
            .MaximumLength(100)
            .When(x => x.City is not null);

        RuleFor(x => x.Street)
            .MaximumLength(200)
            .When(x => x.Street is not null);

        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .When(x => x.PostalCode is not null);

        RuleFor(x => x.Country)
            .MaximumLength(100)
            .When(x => x.Country is not null);

        RuleFor(x => x.FirstLine)
            .MaximumLength(200)
            .When(x => x.FirstLine is not null);

        RuleFor(x => x.SecondLine)
            .MaximumLength(200)
            .When(x => x.SecondLine is not null);
    }
}
