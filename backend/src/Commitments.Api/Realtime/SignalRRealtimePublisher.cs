using Commitments.Api.Hubs;
using Commitments.Shared.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace Commitments.Api.Realtime;

public sealed class SignalRRealtimePublisher : IRealtimePublisher
{
    private const int CurrentSchemaVersion = 1;

    private readonly IHubContext<CommitmentsHub> _hub;
    private readonly ILogger<SignalRRealtimePublisher> _log;

    public SignalRRealtimePublisher(IHubContext<CommitmentsHub> hub, ILogger<SignalRRealtimePublisher> log)
    {
        _hub = hub;
        _log = log;
    }

    public Task PublishToProfileAsync<TPayload>(
        Guid profileId,
        string @event,
        TPayload payload,
        Guid? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        var envelope = new RealtimeMessage<TPayload>(
            CurrentSchemaVersion,
            Guid.NewGuid(),
            @event,
            profileId,
            DateTimeOffset.UtcNow,
            correlationId,
            payload);

        var group = $"profile:{profileId:D}".ToLowerInvariant();

        _log.LogDebug("Realtime publish {Event} → {Group} ({MessageId})", @event, group, envelope.MessageId);

        return _hub.Clients.Group(group).SendAsync("message", envelope, cancellationToken);
    }
}
