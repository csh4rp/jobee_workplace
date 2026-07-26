namespace Jobee.Workplace.Jobs.Domain.Shared;

public record Address
{
    public string? City { get; init; }
    
    public string? Street { get; init; }
    
    public string? PostalCode { get; init; }
    
    public string? Country { get; init; }
        
    public string? FirstLine { get; init; }
        
    public string? SecondLine { get; init; }
}