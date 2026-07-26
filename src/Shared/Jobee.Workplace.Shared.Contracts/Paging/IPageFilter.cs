namespace Jobee.Workplace.Shared.Contracts.Paging;

public interface IPageFilter
{
    public int PageNumber { get; init; }
    
    public int PageSize { get; init; }
}