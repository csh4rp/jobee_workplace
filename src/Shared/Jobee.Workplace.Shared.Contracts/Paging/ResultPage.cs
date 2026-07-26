using System.Collections;

namespace Jobee.Workplace.Shared.Contracts.Paging;

public record ResultPage<T> : IEnumerable<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }
    
    public required int PageNumber { get; init; }

    public required int PageSize { get; init; }

    public required int TotalCount { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}