namespace Commitments.Shared.Realtime;

public sealed record RealtimeMessage<TPayload>(
    int SchemaVersion,
    Guid MessageId,
    string Event,
    Guid ProfileId,
    DateTimeOffset OccurredAt,
    Guid? CorrelationId,
    TPayload Payload);
