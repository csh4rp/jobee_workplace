using FluentValidation;
using Jobee.Workplace.Jobs.Contracts.Categories.Updating;

namespace Jobee.Workplace.Jobs.Application.Categories.Updating;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ParentId)
            .Must(parentId => !parentId.HasValue || parentId.Value != Guid.Empty);
        
        RuleFor(x => x)
            .Must(command => command.Id != command.ParentId)
            .When(x => x.ParentId.HasValue);
    }
}
