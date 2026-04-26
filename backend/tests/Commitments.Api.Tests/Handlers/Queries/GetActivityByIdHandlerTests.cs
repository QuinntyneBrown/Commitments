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

public class GetActivityByIdHandlerTests
{
    [Fact]
    public async Task Handle_ExistingActivity_ReturnsActivity()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type1" };
        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Behaviour1",
            BehaviourType = behaviourType,
            BehaviourTypeId = behaviourType.BehaviourTypeId
        };
        var activity = new Activity
        {
            ActivityId = activityId,
            ProfileId = Guid.NewGuid(),
            BehaviourId = behaviour.BehaviourId,
            PerformedOn = DateTime.UtcNow,
            Description = "Test activity",
            Behaviour = behaviour
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var activities = new List<Activity> { activity };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(activities);
        mockContext.Setup(c => c.Activities).Returns(mockDbSet.Object);

        var handler = new GetActivityByIdHandler(mockContext.Object);
        var request = new GetActivityByIdRequest { ActivityId = activityId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Activity.Should().NotBeNull();
        result.Activity.ActivityId.Should().Be(activityId);
        result.Activity.Description.Should().Be("Test activity");
    }

    [Fact]
    public void GetActivityByIdRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new GetActivityByIdRequest { ActivityId = id };

        request.ActivityId.Should().Be(id);
    }

    [Fact]
    public void GetActivityByIdResponse_Properties_CanBeSetAndRetrieved()
    {
        var dto = new ActivityDto { ActivityId = Guid.NewGuid() };
        var response = new GetActivityByIdResponse { Activity = dto };

        response.Activity.Should().BeSameAs(dto);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new GetActivityByIdValidator();
        var request = new GetActivityByIdRequest { ActivityId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new GetActivityByIdValidator();
        var request = new GetActivityByIdRequest { ActivityId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
