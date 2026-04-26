using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.GoalProgress;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetGoalProgressLast14HandlerTests
{
    [Fact]
    public async Task Handle_ReturnsExactly14Points_OldestFirst()
    {
        var profileId = Guid.NewGuid();
        var commitmentId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();

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

        var handler = new GetGoalProgressLast14Handler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressLast14Request { GoalId = commitmentId, ProfileId = profileId },
            CancellationToken.None);

        response.Points.Should().HaveCount(14);
        var dates = response.Points.Select(p => p.Date).ToList();
        dates.Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task Handle_CountsActivitiesForEachDay()
    {
        var profileId = Guid.NewGuid();
        var commitmentId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();
        var today = DateTime.UtcNow.Date;

        var commitment = new Commitment
        {
            CommitmentId = commitmentId,
            ProfileId = profileId,
            BehaviourId = behaviourId
        };

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = behaviourId, PerformedOn = today },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = behaviourId, PerformedOn = today },
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = behaviourId, PerformedOn = today.AddDays(-3) }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetGoalProgressLast14Handler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressLast14Request { GoalId = commitmentId, ProfileId = profileId },
            CancellationToken.None);

        var todayPoint = response.Points.Single(p => p.Date == today.ToString("yyyy-MM-dd"));
        todayPoint.Completed.Should().Be(2);

        var threeDaysAgoPoint = response.Points.Single(p => p.Date == today.AddDays(-3).ToString("yyyy-MM-dd"));
        threeDaysAgoPoint.Completed.Should().Be(1);
    }

    [Fact]
    public async Task Handle_UnknownGoalId_ReturnsAllZeroes()
    {
        var profileId = Guid.NewGuid();

        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetGoalProgressLast14Handler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressLast14Request { GoalId = Guid.NewGuid(), ProfileId = profileId },
            CancellationToken.None);

        response.Points.Should().HaveCount(14);
        response.Points.All(p => p.Completed == 0).Should().BeTrue();
    }
}
