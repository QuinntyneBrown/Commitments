using Commitments.Data;
using Commitments.Shared;
using Commitments.Shared.Realtime;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Realtime;

public sealed class GoalProgressUpdatedRealtimeNotifier : IHostedService
{
    private const int DefaultTarget = 30;

    private readonly IEventBus _bus;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRealtimePublisher _publisher;
    private readonly ILogger<GoalProgressUpdatedRealtimeNotifier> _log;

    public GoalProgressUpdatedRealtimeNotifier(
        IEventBus bus,
        IServiceScopeFactory scopeFactory,
        IRealtimePublisher publisher,
        ILogger<GoalProgressUpdatedRealtimeNotifier> log)
    {
        _bus = bus;
        _scopeFactory = scopeFactory;
        _publisher = publisher;
        _log = log;
    }

    public Task StartAsync(CancellationToken cancellationToken) =>
        _bus.SubscribeAsync<ActivityRecordedEvent>(HandleAsync, cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task HandleAsync(ActivityRecordedEvent evt)
    {
        using var scope = _scopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<ICommitmentsDbContext>();

        var commitment = await ctx.Commitments
            .FirstOrDefaultAsync(c => c.BehaviourId == evt.BehaviourId && c.ProfileId == evt.ProfileId);

        if (commitment is null)
        {
            _log.LogDebug("No commitment for behaviour {BehaviourId} on profile {ProfileId}; skipping push",
                evt.BehaviourId, evt.ProfileId);
            return;
        }

        var count = await ctx.Activities
            .Where(a => a.ProfileId == evt.ProfileId && a.BehaviourId == commitment.BehaviourId)
            .CountAsync();

        var payload = new GoalProgressUpdatedPayload(
            GoalId: commitment.CommitmentId,
            BehaviourId: evt.BehaviourId,
            Count: count,
            Target: DefaultTarget,
            Percent: ClampPercent(count, DefaultTarget),
            AsOf: DateTimeOffset.UtcNow,
            Date: DateOnly.FromDateTime(evt.PerformedOn.UtcDateTime),
            Reason: ReasonString(evt.Reason),
            SourceActivityId: evt.ActivityId);

        await _publisher.PublishToProfileAsync(evt.ProfileId, "goalProgressUpdated", payload);
    }

    private static int ClampPercent(int count, int target)
    {
        if (target <= 0) return 0;
        var pct = (int)Math.Round(count * 100.0 / target);
        return Math.Max(0, Math.Min(100, pct));
    }

    private static string ReasonString(ActivityChangeReason reason) => reason switch
    {
        ActivityChangeReason.Created => "activityCreated",
        ActivityChangeReason.Updated => "activityUpdated",
        ActivityChangeReason.Deleted => "activityDeleted",
        _ => "commitmentUpdated"
    };
}
