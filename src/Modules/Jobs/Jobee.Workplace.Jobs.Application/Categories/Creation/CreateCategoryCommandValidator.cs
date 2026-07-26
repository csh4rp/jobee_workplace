using FluentValidation;
using Jobee.Workplace.Jobs.Contracts.Categories.Creation;

namespace Jobee.Workplace.Jobs.Application.Categories.Creation;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ParentId)
            .Must(parentId => !parentId.HasValue || parentId.Value != Guid.Empty);
    }
}
