using Commitments.Features.Activity;
using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveActivityHandlerTests
{
    [Fact]
    public async Task Handle_ExistingActivity_RemovesAndSaves()
    {
        // Arrange
        var activityId = Guid.NewGuid();
        var existingActivity = new Activity { ActivityId = activityId };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var activities = new List<Activity> { existingActivity };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(activities);
        mockDbSet.Setup(d => d.FindAsync(activityId)).ReturnsAsync(existingActivity);
        mockContext.Setup(c => c.Activities).Returns(mockDbSet.Object);

        var handler = new RemoveActivityCommandHandler(mockContext.Object);
        var request = new RemoveActivityRequest { ActivityId = activityId };

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        mockDbSet.Verify(d => d.Remove(existingActivity), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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
