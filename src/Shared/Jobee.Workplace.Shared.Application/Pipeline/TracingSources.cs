using System.Diagnostics;

namespace Jobee.Workplace.Shared.Application.Pipeline;

internal abstract class TracingSources
{
    public static readonly ActivitySource Default = new("Jobee.Workplace-Operation");
}