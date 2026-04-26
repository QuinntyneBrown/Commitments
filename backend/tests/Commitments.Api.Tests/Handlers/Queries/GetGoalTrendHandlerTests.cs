using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.GoalProgress;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetGoalTrendHandlerTests
{
    [Fact]
    public async Task Handle_DefaultsTo30DayWindow()
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

        var handler = new GetGoalTrendHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalTrendRequest { GoalId = commitmentId, ProfileId = profileId },
            CancellationToken.None);

        response.WindowDays.Should().Be(30);
        response.Points.Should().HaveCount(30);
        response.GoalId.Should().Be(commitmentId);
    }

    [Fact]
    public async Task Handle_RespectsWindowDaysParameter()
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

        var handler = new GetGoalTrendHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalTrendRequest { GoalId = commitmentId, ProfileId = profileId, WindowDays = 7 },
            CancellationToken.None);

        response.WindowDays.Should().Be(7);
        response.Points.Should().HaveCount(7);
    }

    [Fact]
    public async Task Handle_PopulatesPercentagesAndSummary()
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

        var activities = Enumerable.Range(0, 6).Select(_ => new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = behaviourId,
            PerformedOn = today
        }).ToList();

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetGoalTrendHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalTrendRequest { GoalId = commitmentId, ProfileId = profileId, WindowDays = 7 },
            CancellationToken.None);

        var todayPoint = response.Points.Last();
        todayPoint.Date.Should().Be(today.ToString("yyyy-MM-dd"));
        todayPoint.Completed.Should().Be(6);
        todayPoint.Percentage.Should().Be(20);

        response.CurrentPercentage.Should().Be(20);
        response.PeakPercentage.Should().Be(20);
        response.LowPercentage.Should().Be(0);
    }
}
