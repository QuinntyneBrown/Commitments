using Commitments.Features.Activity;
using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Shared;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveActivityHandlerTests
{
    private readonly Mock<IEventBus> _bus = new();

    [Fact]
    public async Task Handle_ExistingActivity_RemovesAndSaves()
    {
        var activityId = Guid.NewGuid();
        var existingActivity = new Activity { ActivityId = activityId };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var activities = new List<Activity> { existingActivity };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(activities);
        mockDbSet.Setup(d => d.FindAsync(activityId)).ReturnsAsync(existingActivity);
        mockContext.Setup(c => c.Activities).Returns(mockDbSet.Object);

        var handler = new RemoveActivityCommandHandler(mockContext.Object, _bus.Object);
        var request = new RemoveActivityRequest { ActivityId = activityId };

        await handler.Handle(request, CancellationToken.None);

        mockDbSet.Verify(d => d.Remove(existingActivity), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingActivity_PublishesActivityRecordedEvent_WithDeletedReason()
    {
        var activityId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();
        var existingActivity = new Activity
        {
            ActivityId = activityId,
            ProfileId = profileId,
            BehaviourId = behaviourId,
            PerformedOn = DateTime.UtcNow
        };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var activities = new List<Activity> { existingActivity };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(activities);
        mockDbSet.Setup(d => d.FindAsync(activityId)).ReturnsAsync(existingActivity);
        mockContext.Setup(c => c.Activities).Returns(mockDbSet.Object);

        var handler = new RemoveActivityCommandHandler(mockContext.Object, _bus.Object);

        await handler.Handle(new RemoveActivityRequest { ActivityId = activityId }, CancellationToken.None);

        _bus.Verify(b => b.PublishAsync(
            It.Is<ActivityRecordedEvent>(e =>
                e.ActivityId == activityId &&
                e.ProfileId == profileId &&
                e.BehaviourId == behaviourId &&
                e.Reason == ActivityChangeReason.Deleted),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void RemoveActivityRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new RemoveActivityRequest { ActivityId = id };

        request.ActivityId.Should().Be(id);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new RemoveActivityCommandValidator();
        var request = new RemoveActivityRequest { ActivityId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new RemoveActivityCommandValidator();
        var request = new RemoveActivityRequest { ActivityId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
