using Jobee.Workplace.Jobs.Contracts.Categories.Shared;
using MediatR;

namespace Jobee.Workplace.Jobs.Contracts.Categories.Queries;

public record GetCategoryQuery : IRequest<CategoryModel>
{
    public required Guid CategoryId { get; init; }
}