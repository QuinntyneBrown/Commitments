using Commitments.Api.Realtime;
using Commitments.Shared;
using Commitments.Shared.Realtime;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Realtime;

public class DashboardTileInvalidationNotifierTests
{
    private readonly Mock<IRealtimePublisher> _publisher = new();
    private readonly Mock<IEventBus> _bus = new();
    private readonly Guid _profileId = Guid.NewGuid();

    private DashboardTileInvalidationNotifier CreateNotifier() =>
        new(_bus.Object, _publisher.Object,
            NullLogger<DashboardTileInvalidationNotifier>.Instance);

    [Fact]
    public async Task ActivityRecordedEvent_PublishesActivityChangedToProfile()
    {
        var notifier = CreateNotifier();
        var performedOn = DateTimeOffset.Parse("2026-04-26T12:00:00Z");

        await notifier.HandleActivityAsync(new ActivityRecordedEvent
        {
            ActivityId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            ProfileId = _profileId,
            PerformedOn = performedOn,
            Reason = ActivityChangeReason.Created
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "dashboardTileDataInvalidated",
            It.Is<DashboardTileDataInvalidatedPayload>(payload =>
                payload.Reason == "activityChanged" &&
                payload.Datasets.Contains("dailyResults") &&
                payload.Datasets.Contains("weeklyFocus") &&
                payload.Datasets.Contains("monthlyProgress") &&
                payload.Datasets.Contains("relations") &&
                payload.Datasets.Contains("goalTrend") &&
                payload.From == DateOnly.FromDateTime(performedOn.UtcDateTime) &&
                payload.To == DateOnly.FromDateTime(performedOn.UtcDateTime)),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CommitmentChangedEvent_PublishesCommitmentChangedToProfile()
    {
        var notifier = CreateNotifier();
        var commitmentId = Guid.NewGuid();

        await notifier.HandleCommitmentAsync(new CommitmentChangedEvent
        {
            CommitmentId = commitmentId,
            ProfileId = _profileId,
            Kind = ChangeKind.Updated
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "dashboardTileDataInvalidated",
            It.Is<DashboardTileDataInvalidatedPayload>(payload =>
                payload.Reason == "commitmentChanged" &&
                payload.AffectedCommitmentIds.Contains(commitmentId) &&
                payload.Datasets.Contains("dailyResults")),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ToDoChangedEvent_PublishesOnlyOutstandingTodosDataset()
    {
        var notifier = CreateNotifier();
        var toDoId = Guid.NewGuid();

        await notifier.HandleToDoAsync(new ToDoChangedEvent
        {
            ToDoId = toDoId,
            ProfileId = _profileId,
            Kind = ChangeKind.Completed
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "dashboardTileDataInvalidated",
            It.Is<DashboardTileDataInvalidatedPayload>(payload =>
                payload.Reason == "toDoChanged" &&
                payload.Datasets.Count == 1 &&
                payload.Datasets[0] == "outstandingTodos" &&
                payload.AffectedToDoIds.Contains(toDoId)),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FrequencyChangedEvent_PublishesFrequencyChanged()
    {
        var notifier = CreateNotifier();
        var commitmentId = Guid.NewGuid();

        await notifier.HandleFrequencyAsync(new FrequencyChangedEvent
        {
            CommitmentId = commitmentId,
            ProfileId = _profileId
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "dashboardTileDataInvalidated",
            It.Is<DashboardTileDataInvalidatedPayload>(payload =>
                payload.Reason == "frequencyChanged" &&
                payload.AffectedCommitmentIds.Contains(commitmentId)),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProfileCreatedEvent_PublishesProfileChanged_WithAllSixDatasets()
    {
        var notifier = CreateNotifier();

        await notifier.HandleProfileAsync(new ProfileCreatedEvent { ProfileId = _profileId });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "dashboardTileDataInvalidated",
            It.Is<DashboardTileDataInvalidatedPayload>(payload =>
                payload.Reason == "profileChanged" &&
                payload.Datasets.Count == 6),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task StartAsync_SubscribesToAllRelevantEvents()
    {
        var notifier = CreateNotifier();

        await notifier.StartAsync(CancellationToken.None);

        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<ActivityRecordedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<CommitmentChangedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<FrequencyChangedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<ToDoChangedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<ProfileCreatedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
        _bus.Verify(b => b.SubscribeAsync(It.IsAny<Func<ProfileDeletedEvent, Task>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
