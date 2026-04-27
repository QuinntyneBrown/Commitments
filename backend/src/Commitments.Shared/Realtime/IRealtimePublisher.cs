namespace Commitments.Shared.Realtime;

public interface IRealtimePublisher
{
    Task PublishToProfileAsync<TPayload>(
        Guid profileId,
        string @event,
        TPayload payload,
        Guid? correlationId = null,
        CancellationToken cancellationToken = default);
}
