using FluentValidation;
using Jobee.Workplace.Jobs.Application.Offers.Shared;
using Jobee.Workplace.Jobs.Contracts.Offers.Updating;

namespace Jobee.Workplace.Jobs.Application.Offers.Updating;

public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
{
    public UpdateOfferCommandValidator()
    {
        RuleFor(x => x.OfferId)
            .NotEmpty();

        RuleFor(x => x.CompanyId)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(x => x.WorkType)
            .IsInEnum();

        RuleFor(x => x.ContractType)
            .IsInEnum();

        RuleFor(x => x.Requirements)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Requirements)
            .SetValidator(new OfferRequirementModelValidator());

        RuleFor(x => x.Locations)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Locations)
            .SetValidator(new OfferLocationModelValidator());
    }
}
