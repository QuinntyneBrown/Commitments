using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using Commitments.Features.MonthlyProgress;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetMonthlyProgressHandlerTests
{
    private static (Commitment commitment, Behaviour behaviour) BuildDailyCommitment(Guid profileId)
    {
        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type" };
        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Behaviour",
            BehaviourType = behaviourType,
            BehaviourTypeId = behaviourType.BehaviourTypeId
        };
        var frequencyType = new FrequencyType { FrequencyTypeId = Guid.NewGuid(), Name = "per day" };
        var frequency = new Frequency
        {
            FrequencyId = Guid.NewGuid(),
            FrequencyTypeId = frequencyType.FrequencyTypeId,
            FrequencyType = frequencyType
        };
        var commitmentFrequency = new CommitmentFrequency
        {
            CommitmentFrequencyId = Guid.NewGuid(),
            FrequencyId = frequency.FrequencyId,
            Frequency = frequency
        };
        var commitment = new Commitment
        {
            CommitmentId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = behaviour.BehaviourId,
            Behaviour = behaviour,
            CommitmentFrequencies = new List<CommitmentFrequency> { commitmentFrequency }
        };
        return (commitment, behaviour);
    }

    [Fact]
    public async Task Handle_NoActivities_ReturnsFourEmptyBuckets()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Buckets.Should().HaveCount(4);
        response.Buckets.Should().AllSatisfy(b => b.Completed.Should().Be(0));
        response.WindowDays.Should().Be(30);
        response.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_BucketsBoundaries_AreSevenDaysApartEndingAtAsOf()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 0, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Buckets[0].WeekStart.Should().Be("2026-03-30");
        response.Buckets[0].WeekEnd.Should().Be("2026-04-06");
        response.Buckets[1].WeekStart.Should().Be("2026-04-06");
        response.Buckets[1].WeekEnd.Should().Be("2026-04-13");
        response.Buckets[2].WeekStart.Should().Be("2026-04-13");
        response.Buckets[2].WeekEnd.Should().Be("2026-04-20");
        response.Buckets[3].WeekStart.Should().Be("2026-04-20");
        response.Buckets[3].WeekEnd.Should().Be("2026-04-27");
    }

    [Fact]
    public async Task Handle_ActivitiesInLastBucket_CountedCorrectly()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var (c1, b1) = BuildDailyCommitment(profileId);

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b1.BehaviourId, PerformedOn = asOf.UtcDateTime.AddDays(-1) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b1.BehaviourId, PerformedOn = asOf.UtcDateTime.AddDays(-2) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Buckets[3].Completed.Should().Be(2);
        response.Buckets[3].Target.Should().Be(7);
        response.Buckets[3].Percentage.Should().Be(29);
        response.Buckets[0].Completed.Should().Be(0);
        response.Buckets[1].Completed.Should().Be(0);
        response.Buckets[2].Completed.Should().Be(0);
        response.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ActivitiesOutsideWindow_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var (c1, b1) = BuildDailyCommitment(profileId);

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b1.BehaviourId, PerformedOn = asOf.UtcDateTime.AddDays(-29) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b1.BehaviourId, PerformedOn = asOf.UtcDateTime.AddDays(1) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Buckets.Sum(b => b.Completed).Should().Be(0);
    }

    [Fact]
    public async Task Handle_PercentageClampedAt100()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var (c1, b1) = BuildDailyCommitment(profileId);

        // Way more activities than target (target=7 with 1 daily commitment)
        var activities = Enumerable.Range(1, 50)
            .Select(i => new Activity
            {
                ActivityId = Guid.NewGuid(),
                ProfileId = profileId,
                BehaviourId = b1.BehaviourId,
                PerformedOn = asOf.UtcDateTime.AddDays(-1).AddHours(-i)
            })
            .ToList();

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Buckets[3].Percentage.Should().Be(100);
    }

    [Fact]
    public async Task Handle_NoDailyCommitments_TargetIsZeroAndPercentageZero()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Buckets.Should().AllSatisfy(b =>
        {
            b.Target.Should().Be(0);
            b.Percentage.Should().Be(0);
        });
    }

    [Fact]
    public async Task Handle_NullAsOf_ModeIsLive()
    {
        var profileId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = null },
            CancellationToken.None);

        response.Mode.Should().Be("live");
        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_AsOfProvided_ModeIsReview()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 22, 12, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Mode.Should().Be("review");
        response.AsOf.Should().Be(asOf);
    }

    [Fact]
    public async Task Handle_FutureAsOf_ClampedToUtcNow()
    {
        var profileId = Guid.NewGuid();
        var future = DateTimeOffset.UtcNow.AddYears(10);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetMonthlyProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetMonthlyProgressRequest { ProfileId = profileId, AsOf = future },
            CancellationToken.None);

        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }
}
