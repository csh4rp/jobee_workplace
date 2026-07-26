using System.Collections.Concurrent;

namespace Jobee.Workplace.Shared.Application.Pipeline;

internal sealed class TransactionalTypesDictionary
{
    private static readonly ConcurrentDictionary<Type, bool> TransactionalTypes = new();

    public static bool? ShouldUseTransaction(Type type)
    {
        if (TransactionalTypes.TryGetValue(type, out var shouldUseTransaction))
        {
            return shouldUseTransaction;
        }

        return null;
    }

    public static void Add(Type type, bool shouldUseTransaction) =>
        TransactionalTypes.TryAdd(type, shouldUseTransaction);

}