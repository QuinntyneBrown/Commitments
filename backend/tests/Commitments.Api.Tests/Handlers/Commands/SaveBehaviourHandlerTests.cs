using Commitments.Features.Behaviour;
using Commitments.Data;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveBehaviourHandlerTests
{
    [Fact]
    public async Task Handle_NewBehaviour_AddsAndSaves()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviours = new List<Behaviour>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviours);
        mockContext.Setup(c => c.Behaviours).Returns(mockDbSet.Object);

        var handler = new SaveBehaviourCommandHandler(mockContext.Object);
        var request = new SaveBehaviourRequest
        {
            Behaviour = new BehaviourDto
            {
                Name = "Test Behaviour",
                Description = "Test Description",
                BehaviourTypeId = Guid.NewGuid()
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        behaviours.Should().HaveCount(1);
        behaviours[0].Name.Should().Be("Test Behaviour");
        behaviours[0].Description.Should().Be("Test Description");
        behaviours[0].BehaviourTypeId.Should().Be(request.Behaviour.BehaviourTypeId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingBehaviour_UpdatesAndSaves()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var behaviourTypeId = Guid.NewGuid();
        var existingBehaviour = new Behaviour
        {
            BehaviourId = existingId,
            Name = "Old Name",
            Description = "Old Description",
            BehaviourTypeId = behaviourTypeId
        };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviours = new List<Behaviour> { existingBehaviour };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviours);
        mockDbSet.Setup(d => d.FindAsync(existingId)).ReturnsAsync(existingBehaviour);
        mockContext.Setup(c => c.Behaviours).Returns(mockDbSet.Object);

        var newBehaviourTypeId = Guid.NewGuid();
        var handler = new SaveBehaviourCommandHandler(mockContext.Object);
        var request = new SaveBehaviourRequest
        {
            Behaviour = new BehaviourDto
            {
                BehaviourId = existingId,
                Name = "Updated Name",
                Description = "Updated Description",
                BehaviourTypeId = newBehaviourTypeId
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.BehaviourId.Should().Be(existingId);
        existingBehaviour.Name.Should().Be("Updated Name");
        existingBehaviour.Description.Should().Be("Updated Description");
        existingBehaviour.BehaviourTypeId.Should().Be(newBehaviourTypeId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SaveBehaviourRequest_Properties_CanBeSetAndRetrieved()
    {
        var dto = new BehaviourDto { BehaviourId = Guid.NewGuid(), Name = "Test" };
        var request = new SaveBehaviourRequest { Behaviour = dto };

        request.Behaviour.Should().BeSameAs(dto);
    }

    [Fact]
    public void SaveBehaviourResponse_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var response = new SaveBehaviourResponse { BehaviourId = id };

        response.BehaviourId.Should().Be(id);
    }
}
