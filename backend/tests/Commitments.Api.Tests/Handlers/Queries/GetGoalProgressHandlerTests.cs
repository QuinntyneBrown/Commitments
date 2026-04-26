using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.GoalProgress;
using Commitments.Testing.Common;
using FluentAssertions;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetGoalProgressHandlerTests
{
    [Fact]
    public async Task Handle_NoActivitiesForCommitment_ReturnsZeroCount()
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

        var handler = new GetGoalProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressRequest { GoalId = commitmentId, ProfileId = profileId },
            CancellationToken.None);

        response.Should().NotBeNull();
        response.GoalId.Should().Be(commitmentId);
        response.Count.Should().Be(0);
        response.Target.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_ActivitiesForCommitment_ReturnsActivityCount()
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

        var activities = Enumerable.Range(0, 3).Select(_ => new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = behaviourId,
            PerformedOn = DateTime.UtcNow
        }).ToList();

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetGoalProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressRequest { GoalId = commitmentId, ProfileId = profileId },
            CancellationToken.None);

        response.Count.Should().Be(3);
    }

    [Fact]
    public async Task Handle_ActivitiesFromDifferentProfile_AreExcluded()
    {
        var profileId = Guid.NewGuid();
        var otherProfileId = Guid.NewGuid();
        var commitmentId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();

        var commitment = new Commitment
        {
            CommitmentId = commitmentId,
            ProfileId = profileId,
            BehaviourId = behaviourId
        };

        var activities = new List<Activity>
        {
            new() { ActivityId = Guid.NewGuid(), ProfileId = profileId, BehaviourId = behaviourId, PerformedOn = DateTime.UtcNow },
            new() { ActivityId = Guid.NewGuid(), ProfileId = otherProfileId, BehaviourId = behaviourId, PerformedOn = DateTime.UtcNow }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        mockContext.Setup(c => c.Commitments)
            .Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment }).Object);
        mockContext.Setup(c => c.Activities)
            .Returns(MockDbSetFactory.CreateMockDbSet(activities).Object);

        var handler = new GetGoalProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressRequest { GoalId = commitmentId, ProfileId = profileId },
            CancellationToken.None);

        response.Count.Should().Be(1);
    }

    [Fact]
    public async Task Handle_UnknownGoalId_ReturnsZeroCount()
    {
        var profileId = Guid.NewGuid();
        var unknownGoalId = Guid.NewGuid();

        var mockContext = MockCommitmentsDbContextFactory.Create();

        var handler = new GetGoalProgressHandler(mockContext.Object);
        var response = await handler.Handle(
            new GetGoalProgressRequest { GoalId = unknownGoalId, ProfileId = profileId },
            CancellationToken.None);

        response.Count.Should().Be(0);
    }
}
