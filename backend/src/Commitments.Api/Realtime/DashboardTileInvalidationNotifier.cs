using Commitments.Shared;
using Commitments.Shared.Realtime;

namespace Commitments.Api.Realtime;

public sealed class DashboardTileInvalidationNotifier : IHostedService
{
    private static readonly IReadOnlyList<Guid> Empty = Array.Empty<Guid>();

    private readonly IEventBus _bus;
    private readonly IRealtimePublisher _publisher;
    private readonly ILogger<DashboardTileInvalidationNotifier> _log;

    public DashboardTileInvalidationNotifier(
        IEventBus bus,
        IRealtimePublisher publisher,
        ILogger<DashboardTileInvalidationNotifier> log)
    {
        _bus = bus;
        _publisher = publisher;
        _log = log;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _bus.SubscribeAsync<ActivityRecordedEvent>(HandleActivityAsync, cancellationToken);
        await _bus.SubscribeAsync<CommitmentChangedEvent>(HandleCommitmentAsync, cancellationToken);
        await _bus.SubscribeAsync<FrequencyChangedEvent>(HandleFrequencyAsync, cancellationToken);
        await _bus.SubscribeAsync<ToDoChangedEvent>(HandleToDoAsync, cancellationToken);
        await _bus.SubscribeAsync<ProfileCreatedEvent>(HandleProfileAsync, cancellationToken);
        await _bus.SubscribeAsync<ProfileDeletedEvent>(HandleProfileAsync, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task HandleActivityAsync(ActivityRecordedEvent evt)
    {
        var date = DateOnly.FromDateTime(evt.PerformedOn.UtcDateTime);
        return Publish(evt.ProfileId,
            DashboardTileDataset.AllExceptToDos,
            "activityChanged",
            affectedCommitmentIds: Empty,
            affectedToDoIds: Empty,
            from: date,
            to: date);
    }

    public Task HandleCommitmentAsync(CommitmentChangedEvent evt) =>
        Publish(evt.ProfileId,
            DashboardTileDataset.AllExceptToDos,
            "commitmentChanged",
            affectedCommitmentIds: new[] { evt.CommitmentId },
            affectedToDoIds: Empty,
            from: Today(),
            to: Today());

    public Task HandleFrequencyAsync(FrequencyChangedEvent evt) =>
        Publish(evt.ProfileId,
            DashboardTileDataset.AllExceptToDos,
            "frequencyChanged",
            affectedCommitmentIds: new[] { evt.CommitmentId },
            affectedToDoIds: Empty,
            from: Today(),
            to: Today());

    public Task HandleToDoAsync(ToDoChangedEvent evt) =>
        Publish(evt.ProfileId,
            new[] { DashboardTileDataset.OutstandingTodos },
            "toDoChanged",
            affectedCommitmentIds: Empty,
            affectedToDoIds: new[] { evt.ToDoId },
            from: Today(),
            to: Today());

    public Task HandleProfileAsync(ProfileCreatedEvent evt) =>
        Publish(evt.ProfileId, DashboardTileDataset.All, "profileChanged",
            Empty, Empty, Today(), Today());

    public Task HandleProfileAsync(ProfileDeletedEvent evt) =>
        Publish(evt.ProfileId, DashboardTileDataset.All, "profileChanged",
            Empty, Empty, Today(), Today());

    private static DateOnly Today() => DateOnly.FromDateTime(DateTime.UtcNow);

    private Task Publish(
        Guid profileId,
        IReadOnlyList<string> datasets,
        string reason,
        IReadOnlyList<Guid> affectedCommitmentIds,
        IReadOnlyList<Guid> affectedToDoIds,
        DateOnly? from,
        DateOnly? to)
    {
        var payload = new DashboardTileDataInvalidatedPayload(
            Datasets: datasets,
            AffectedGoalIds: Empty,
            AffectedCommitmentIds: affectedCommitmentIds,
            AffectedToDoIds: affectedToDoIds,
            From: from,
            To: to,
            Reason: reason);

        _log.LogDebug("Tile invalidation {Reason} → profile {ProfileId}", reason, profileId);

        return _publisher.PublishToProfileAsync(profileId, "dashboardTileDataInvalidated", payload);
    }
}
