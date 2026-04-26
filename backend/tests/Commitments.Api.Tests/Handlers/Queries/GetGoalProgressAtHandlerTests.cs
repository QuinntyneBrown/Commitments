using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.GoalProgress;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetGoalProgressAtHandlerTests
{
    [Fact]
    public async Task Handle_OnlyCountsActivitiesAtOrBeforeAsOf()
    {
        var profileId = Guid.NewGuid();
        var commitmentId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();
        var asOf = new DateTimeOffset(2026, 4, 10, 0, 0, 0, TimeSpan.Zero);

        var commitment = new Commitment
        {
            CommitmentId = commitmentId,
            ProfileId = profileId,
            BehaviourId = behaviourId
        };

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = behaviourId, PerformedOn = asOf.UtcDateTime.AddDays(-2) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = behaviourId, PerformedOn = asOf.UtcDateTime.AddDays(-1) },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = behaviourId, PerformedOn = asOf.UtcDateTime.AddDays(1) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetGoalProgressAtHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressAtRequest { GoalId = commitmentId, ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Count.Should().Be(2);
        response.AsOf.Should().Be(asOf);
        response.GoalId.Should().Be(commitmentId);
        response.Target.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_ClampsFutureAsOfToUtcNow()
    {
        var profileId = Guid.NewGuid();
        var commitmentId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();
        var future = DateTimeOffset.UtcNow.AddYears(10);

        var commitment = new Commitment
        {
            CommitmentId = commitmentId,
            ProfileId = profileId,
            BehaviourId = behaviourId
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Activity>()).Object);

        var handler = new GetGoalProgressAtHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressAtRequest { GoalId = commitmentId, ProfileId = profileId, AsOf = future },
            CancellationToken.None);

        response.AsOf.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_UnknownGoalId_ReturnsZeroCount()
    {
        var profileId = Guid.NewGuid();
        var asOf = DateTimeOffset.UtcNow;
        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetGoalProgressAtHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressAtRequest { GoalId = Guid.NewGuid(), ProfileId = profileId, AsOf = asOf },
            CancellationToken.None);

        response.Count.Should().Be(0);
    }
}
