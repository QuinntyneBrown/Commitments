using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using Commitments.Features.Commitment;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetDailyResultsHandlerTests
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
    public async Task Handle_NoCommitments_ReturnsZeroTotalAndCompleted()
    {
        var profileId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var handler = new GetDailyResultsHandler(mockContext.Object);

        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = null },
            CancellationToken.None);

        response.Total.Should().Be(0);
        response.Completed.Should().Be(0);
    }

    [Fact]
    public async Task Handle_TwoDailyCommitmentsNoActivities_TotalIsTwoCompletedIsZero()
    {
        var profileId = Guid.NewGuid();
        var (c1, _) = BuildDailyCommitment(profileId);
        var (c2, _) = BuildDailyCommitment(profileId);

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1, c2 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Activity>()).Object);

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = null },
            CancellationToken.None);

        response.Total.Should().Be(2);
        response.Completed.Should().Be(0);
    }

    [Fact]
    public async Task Handle_OneCommitmentDoneToday_CompletedIsOne()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 18, 0, 0, TimeSpan.Zero);
        var (c1, b1) = BuildDailyCommitment(profileId);
        var (c2, _) = BuildDailyCommitment(profileId);

        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = b1.BehaviourId,
            PerformedOn = asOf.UtcDateTime.AddHours(-2)
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1, c2 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Activity> { activity }).Object);

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Total.Should().Be(2);
        response.Completed.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ActivityAfterAsOf_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var (c1, b1) = BuildDailyCommitment(profileId);

        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = b1.BehaviourId,
            PerformedOn = asOf.UtcDateTime.AddHours(2)
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Activity> { activity }).Object);

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Completed.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ActivityFromYesterday_NotCounted()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 12, 0, 0, TimeSpan.Zero);
        var (c1, b1) = BuildDailyCommitment(profileId);

        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = b1.BehaviourId,
            PerformedOn = asOf.UtcDateTime.AddDays(-1)
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Activity> { activity }).Object);

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Completed.Should().Be(0);
    }

    [Fact]
    public async Task Handle_MultipleActivitiesSameCommitmentSameDay_CountedOnce()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 27, 23, 0, 0, TimeSpan.Zero);
        var (c1, b1) = BuildDailyCommitment(profileId);

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b1.BehaviourId, PerformedOn = asOf.UtcDateTime.AddHours(-5) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = b1.BehaviourId, PerformedOn = asOf.UtcDateTime.AddHours(-1) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { c1 }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Completed.Should().Be(1);
    }

    [Fact]
    public async Task Handle_NullAsOf_ModeIsLiveAndAsOfIsCurrentUtc()
    {
        var profileId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = null },
            CancellationToken.None);

        response.Mode.Should().Be("live");
        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_AsOfProvided_ModeIsReview()
    {
        var profileId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 20, 15, 0, 0, TimeSpan.Zero);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Mode.Should().Be("review");
        response.AsOf.Should().Be(asOf);
        response.Date.Should().Be("2026-04-20");
    }

    [Fact]
    public async Task Handle_FutureAsOf_ClampedToUtcNow()
    {
        var profileId = Guid.NewGuid();
        var future = DateTimeOffset.UtcNow.AddYears(10);
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetDailyResultsHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetDailyResultsRequest { ProfileId = profileId, AsOf = future },
            CancellationToken.None);

        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }
}
