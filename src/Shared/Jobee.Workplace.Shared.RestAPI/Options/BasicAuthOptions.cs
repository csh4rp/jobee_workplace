using System.Diagnostics.CodeAnalysis;

namespace Jobee.Workplace.Shared.RestAPI.Options;

[ExcludeFromCodeCoverage]
public class BasicAuthOptions
{
    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string[] Paths { get; set; }
}