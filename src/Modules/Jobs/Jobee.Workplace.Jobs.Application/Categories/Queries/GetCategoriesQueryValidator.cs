using FluentValidation;
using Jobee.Workplace.Jobs.Contracts.Categories.Queries;

namespace Jobee.Workplace.Jobs.Application.Categories.Queries;

public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.ParentCategoryId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .When(x => x.ParentCategoryId.HasValue);
    }
}

