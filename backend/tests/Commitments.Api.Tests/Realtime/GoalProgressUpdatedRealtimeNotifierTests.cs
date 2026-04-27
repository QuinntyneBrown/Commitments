using Commitments.Api.Realtime;
using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Shared;
using Commitments.Shared.Realtime;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Realtime;

public class GoalProgressUpdatedRealtimeNotifierTests : IDisposable
{
    private readonly Guid _profileId = Guid.NewGuid();
    private readonly Guid _behaviourId = Guid.NewGuid();
    private readonly Guid _commitmentId = Guid.NewGuid();
    private readonly CommitmentsDbContext _ctx;
    private readonly ServiceProvider _provider;
    private readonly Mock<IRealtimePublisher> _publisher = new();
    private readonly Mock<IEventBus> _bus = new();

    public GoalProgressUpdatedRealtimeNotifierTests()
    {
        var services = new ServiceCollection();
        var dbName = Guid.NewGuid().ToString();
        var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
        services.AddDbContext<CommitmentsDbContext>(o =>
            o.UseInMemoryDatabase(dbName, root));
        services.AddScoped<ICommitmentsDbContext>(sp => sp.GetRequiredService<CommitmentsDbContext>());
        _provider = services.BuildServiceProvider();
        _ctx = _provider.GetRequiredService<CommitmentsDbContext>();
    }

    private GoalProgressUpdatedRealtimeNotifier CreateNotifier() =>
        new(_bus.Object,
            _provider.GetRequiredService<IServiceScopeFactory>(),
            _publisher.Object,
            NullLogger<GoalProgressUpdatedRealtimeNotifier>.Instance);

    private void SeedCommitmentAndActivities(int activityCount, int target = 30)
    {
        _ctx.Commitments.Add(new Commitment
        {
            CommitmentId = _commitmentId,
            BehaviourId = _behaviourId,
            ProfileId = _profileId
        });
        for (var i = 0; i < activityCount; i++)
        {
            _ctx.Activities.Add(new Activity
            {
                ActivityId = Guid.NewGuid(),
                BehaviourId = _behaviourId,
                ProfileId = _profileId,
                PerformedOn = DateTime.UtcNow,
                Description = "seeded"
            });
        }
        _ctx.SaveChanges();
    }

    [Fact]
    public async Task HandleAsync_PublishesGoalProgressUpdated_ToProfileGroup()
    {
        SeedCommitmentAndActivities(activityCount: 5);
        var notifier = CreateNotifier();
        var evt = new ActivityRecordedEvent
        {
            ActivityId = Guid.NewGuid(),
            BehaviourId = _behaviourId,
            ProfileId = _profileId,
            PerformedOn = DateTimeOffset.UtcNow,
            Reason = ActivityChangeReason.Created
        };

        await notifier.HandleAsync(evt);

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "goalProgressUpdated",
            It.Is<GoalProgressUpdatedPayload>(payload =>
                payload.GoalId == _commitmentId &&
                payload.BehaviourId == _behaviourId &&
                payload.Count == 5 &&
                payload.Reason == "activityCreated" &&
                payload.SourceActivityId == evt.ActivityId),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_NoMatchingCommitment_DoesNotPublish()
    {
        var notifier = CreateNotifier();
        var evt = new ActivityRecordedEvent
        {
            ActivityId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            ProfileId = _profileId,
            PerformedOn = DateTimeOffset.UtcNow,
            Reason = ActivityChangeReason.Created
        };

        await notifier.HandleAsync(evt);

        _publisher.Verify(p => p.PublishToProfileAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<GoalProgressUpdatedPayload>(),
            It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(ActivityChangeReason.Created, "activityCreated")]
    [InlineData(ActivityChangeReason.Updated, "activityUpdated")]
    [InlineData(ActivityChangeReason.Deleted, "activityDeleted")]
    public async Task HandleAsync_MapsReasonStrings(ActivityChangeReason reason, string expected)
    {
        SeedCommitmentAndActivities(activityCount: 1);
        var notifier = CreateNotifier();

        await notifier.HandleAsync(new ActivityRecordedEvent
        {
            ActivityId = Guid.NewGuid(),
            BehaviourId = _behaviourId,
            ProfileId = _profileId,
            PerformedOn = DateTimeOffset.UtcNow,
            Reason = reason
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId, "goalProgressUpdated",
            It.Is<GoalProgressUpdatedPayload>(p2 => p2.Reason == expected),
            It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_PercentClampsAt100_WhenCountExceedsTarget()
    {
        SeedCommitmentAndActivities(activityCount: 100);
        var notifier = CreateNotifier();

        await notifier.HandleAsync(new ActivityRecordedEvent
        {
            ActivityId = Guid.NewGuid(),
            BehaviourId = _behaviourId,
            ProfileId = _profileId,
            PerformedOn = DateTimeOffset.UtcNow,
            Reason = ActivityChangeReason.Created
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId, "goalProgressUpdated",
            It.Is<GoalProgressUpdatedPayload>(p2 => p2.Percent == 100),
            It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_PublishesOncePerMatchingCommitment()
    {
        var secondCommitmentId = Guid.NewGuid();
        SeedCommitmentAndActivities(activityCount: 3);
        _ctx.Commitments.Add(new Commitment
        {
            CommitmentId = secondCommitmentId,
            BehaviourId = _behaviourId,
            ProfileId = _profileId
        });
        _ctx.SaveChanges();

        var notifier = CreateNotifier();
        await notifier.HandleAsync(new ActivityRecordedEvent
        {
            ActivityId = Guid.NewGuid(),
            BehaviourId = _behaviourId,
            ProfileId = _profileId,
            PerformedOn = DateTimeOffset.UtcNow,
            Reason = ActivityChangeReason.Created
        });

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId, "goalProgressUpdated",
            It.Is<GoalProgressUpdatedPayload>(x => x.GoalId == _commitmentId),
            It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Once);
        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId, "goalProgressUpdated",
            It.Is<GoalProgressUpdatedPayload>(x => x.GoalId == secondCommitmentId),
            It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task StartAsync_SubscribesToActivityRecordedEventOnTheBus()
    {
        var notifier = CreateNotifier();

        await notifier.StartAsync(CancellationToken.None);

        _bus.Verify(b => b.SubscribeAsync(
            It.IsAny<Func<ActivityRecordedEvent, Task>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    public void Dispose()
    {
        _ctx.Dispose();
        _provider.Dispose();
    }
}
