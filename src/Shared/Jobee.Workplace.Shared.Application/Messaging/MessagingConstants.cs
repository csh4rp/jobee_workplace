namespace Jobee.Workplace.Shared.Application.Messaging;

public abstract class MessagingConstants
{
    public const string ScheduleQueueName = "queue:quartz";

    public const string SubjectHeaderName = "subject";
    public const string AppHeaderName = "app";
    public const string PermissionsHeaderName = "permissions";

    public const string TimestampHeaderName = "timestamp";
    public const string CorrelationIdHeaderName = "X-Correlation-ID";

    public static readonly Uri ScheduleQueueUri = new(ScheduleQueueName);
}