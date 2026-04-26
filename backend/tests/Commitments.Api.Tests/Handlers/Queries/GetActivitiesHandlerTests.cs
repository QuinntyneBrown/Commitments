using Commitments.Features.Activity;
using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetActivitiesHandlerTests
{
    [Fact]
    public async Task Handle_NoActivities_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var handler = new GetActivitiesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetActivitiesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Activities.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithActivities_ReturnsPopulatedList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type1" };
        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Behaviour1",
            BehaviourType = behaviourType,
            BehaviourTypeId = behaviourType.BehaviourTypeId
        };
        var activities = new List<Activity>
        {
            new Activity
            {
                ActivityId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                BehaviourId = behaviour.BehaviourId,
                PerformedOn = DateTime.UtcNow,
                Description = "Test activity 1",
                Behaviour = behaviour
            },
            new Activity
            {
                ActivityId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                BehaviourId = behaviour.BehaviourId,
                PerformedOn = DateTime.UtcNow,
                Description = "Test activity 2",
                Behaviour = behaviour
            }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(activities);
        mockContext.Setup(c => c.Activities).Returns(mockDbSet.Object);

        var handler = new GetActivitiesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetActivitiesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Activities.Should().HaveCount(2);
    }

    [Fact]
    public void GetActivitiesResponse_Properties_CanBeSetAndRetrieved()
    {
        var dtos = new List<ActivityDto> { new ActivityDto { ActivityId = Guid.NewGuid() } };
        var response = new GetActivitiesResponse { Activities = dtos };

        response.Activities.Should().BeSameAs(dtos);
    }
}
