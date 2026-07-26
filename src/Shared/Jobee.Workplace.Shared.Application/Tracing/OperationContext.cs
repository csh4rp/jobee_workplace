namespace Jobee.Workplace.Shared.Application.Tracing;

using System.Diagnostics;

public sealed class OperationContext
{
    private readonly Dictionary<string, string> _tags = [];

    public OperationContext(string correlationId) => CorrelationId = correlationId;

    public string OperationName
    {
        get
        {
            Debug.Assert(Activity.Current is not null);
            return field ?? Activity.Current.OperationName;
        }
        set
        {
            if (field is not null)
            {
                throw new InvalidOperationException("OperationName is already set");
            }

            field = value;
        }
    }

    public string TraceId
    {
        get
        {
            Debug.Assert(Activity.Current is not null);
            return Activity.Current.TraceId.ToString();
        }
    }

    public string CorrelationId { get; }

    public void AddTag(string name, string value)
    {
        _tags.Add(name, value);
        Activity.Current?.AddTag(name, value);
    }

    public IReadOnlyDictionary<string, string> GetTags()
    {
        var tags = new Dictionary<string, string>();

        foreach (var (key, value) in _tags)
        {
            tags.Add(key, value);
        }

        var currentActivity = Activity.Current;
        if (currentActivity is null)
        {
            return tags;
        }

        foreach (var (key, value) in currentActivity.Tags
                     .Where(t => t.Value is not null))
        {
            _ = tags.TryAdd(key, value!);
        }

        return tags;
    }
}
