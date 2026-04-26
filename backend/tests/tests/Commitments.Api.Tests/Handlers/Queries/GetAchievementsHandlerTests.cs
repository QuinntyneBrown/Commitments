using Commitments.Api.Features.Achievement;
using Commitments.Core;
using Commitments.Core.Model.ActivityAggregate;
using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetAchievementsHandlerTests
{
    [Fact]
    public async Task Handle_NoCommitments_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var handler = new GetAchievementsQueryHandler(mockContext.Object);
        var request = new GetAchievementsRequest { ProfileId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Achievements.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_CommitmentWithMatchingActivity_ReturnsAchievement()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();

        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type1" };
        var behaviour = new Behaviour
        {
            BehaviourId = behaviourId,
            Name = "Test Behaviour",
            BehaviourType = behaviourType,
            BehaviourTypeId = behaviourType.BehaviourTypeId
        };

        var frequencyType = new FrequencyType { FrequencyTypeId = Guid.NewGuid(), Name = "per day" };
        var frequency = new Frequency
        {
            FrequencyId = Guid.NewGuid(),
            Frequency = Guid.NewGuid(),
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
            BehaviourId = behaviourId,
            Behaviour = behaviour,
            CommitmentFrequencies = new List<CommitmentFrequency> { commitmentFrequency }
        };

        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = profileId,
            BehaviourId = behaviourId,
            PerformedOn = DateTime.Now,
            Description = "Done today",
            Behaviour = behaviour
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var commitmentDbSet = MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment });
        mockContext.Setup(c => c.Commitments).Returns(commitmentDbSet.Object);

        var activityDbSet = MockDbSetFactory.CreateMockDbSet(new List<Activity> { activity });
        mockContext.Setup(c => c.Activities).Returns(activityDbSet.Object);

        var handler = new GetAchievementsQueryHandler(mockContext.Object);
        var request = new GetAchievementsRequest { ProfileId = profileId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Achievements.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_CommitmentWithNoMatchingActivity_ReturnsEmptyAchievements()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();

        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type1" };
        var behaviour = new Behaviour
        {
            BehaviourId = behaviourId,
            Name = "Test Behaviour",
            BehaviourType = behaviourType,
            BehaviourTypeId = behaviourType.BehaviourTypeId
        };

        var frequencyType = new FrequencyType { FrequencyTypeId = Guid.NewGuid(), Name = "per day" };
        var frequency = new Frequency
        {
            FrequencyId = Guid.NewGuid(),
            Frequency = Guid.NewGuid(),
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
            BehaviourId = behaviourId,
            Behaviour = behaviour,
            CommitmentFrequencies = new List<CommitmentFrequency> { commitmentFrequency }
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var commitmentDbSet = MockDbSetFactory.CreateMockDbSet(new List<Commitment> { commitment });
        mockContext.Setup(c => c.Commitments).Returns(commitmentDbSet.Object);

        // Empty activities -- no matching activity for today
        var activityDbSet = MockDbSetFactory.CreateMockDbSet(new List<Activity>());
        mockContext.Setup(c => c.Activities).Returns(activityDbSet.Object);

        var handler = new GetAchievementsQueryHandler(mockContext.Object);
        var request = new GetAchievementsRequest { ProfileId = profileId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Achievements.Should().BeEmpty();
    }

    [Fact]
    public void GetAchievementsRequest_Properties_CanBeSetAndRetrieved()
    {
        var profileId = Guid.NewGuid();
        var request = new GetAchievementsRequest { ProfileId = profileId };

        request.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public void GetAchievementsResponse_Properties_CanBeSetAndRetrieved()
    {
        var achievements = new List<AchievementDto> { new AchievementDto() };
        var response = new GetAchievementsResponse { Achievements = achievements };

        response.Achievements.Should().BeSameAs(achievements);
    }
}
