using Microsoft.Extensions.Logging;

namespace Jobee.Workplace.Shared.Application.Pipeline;

public static partial class LoggingExtensions
{
    [LoggerMessage(EventId = 3000, Level = LogLevel.Information, Message = "Operation: '{OperationType}' has started")]
    public static partial void OperationStarted(this ILogger logger, string operationType);

    [LoggerMessage(EventId = 3001, Level = LogLevel.Information, Message = "Operation: '{OperationType}' has finished")]
    public static partial void OperationFinished(this ILogger logger, string operationType);

    [LoggerMessage(EventId = 3002, Level = LogLevel.Debug, Message = "Transaction for '{OperationType}' has started")]
    public static partial void TransactionStarted(this ILogger logger, string operationType);

    [LoggerMessage(EventId = 3003, Level = LogLevel.Debug, Message = "Transaction for '{OperationType}' has completed")]
    public static partial void TransactionCompleted(this ILogger logger, string operationType);
}