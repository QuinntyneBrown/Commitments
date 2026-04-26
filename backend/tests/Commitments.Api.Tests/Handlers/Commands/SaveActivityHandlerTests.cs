using Commitments.Api.Features.Activity;
using Commitments.Core;
using Commitments.Core.Model.ActivityAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveActivityHandlerTests
{
    [Fact]
    public async Task Handle_NewActivity_AddsAndSaves()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var activities = new List<Activity>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(activities);
        mockContext.Setup(c => c.Activities).Returns(mockDbSet.Object);

        var handler = new SaveActivityCommandHandler(mockContext.Object);
        var request = new SaveActivityRequest
        {
            Activity = new ActivityDto
            {
                BehaviourId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                PerformedOn = DateTime.UtcNow,
                Description = "Test activity"
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ActivityId.Should().Be(default(Guid));
        activities.Should().HaveCount(1);
        activities[0].BehaviourId.Should().Be(request.Activity.BehaviourId);
        activities[0].ProfileId.Should().Be(request.Activity.ProfileId);
        activities[0].Description.Should().Be("Test activity");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingActivity_UpdatesAndSaves()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var existingActivity = new Activity
        {
            ActivityId = existingId,
            BehaviourId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            Description = "Old description"
        };
        var activities = new List<Activity> { existingActivity };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(activities);
        mockDbSet.Setup(d => d.FindAsync(existingId)).ReturnsAsync(existingActivity);
        mockContext.Setup(c => c.Activities).Returns(mockDbSet.Object);

        var newBehaviourId = Guid.NewGuid();
        var handler = new SaveActivityCommandHandler(mockContext.Object);
        var request = new SaveActivityRequest
        {
            Activity = new ActivityDto
            {
                ActivityId = existingId,
                BehaviourId = newBehaviourId,
                ProfileId = Guid.NewGuid(),
                PerformedOn = DateTime.UtcNow,
                Description = "Updated description"
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ActivityId.Should().Be(existingId);
        existingActivity.BehaviourId.Should().Be(newBehaviourId);
        existingActivity.Description.Should().Be("Updated description");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SaveActivityRequest_Properties_CanBeSetAndRetrieved()
    {
        var dto = new ActivityDto { ActivityId = Guid.NewGuid() };
        var request = new SaveActivityRequest { Activity = dto };

        request.Activity.Should().BeSameAs(dto);
    }

    [Fact]
    public void SaveActivityResponse_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var response = new SaveActivityResponse { ActivityId = id };

        response.ActivityId.Should().Be(id);
    }
}
