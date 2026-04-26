using Commitments.Api.Features.Behaviour;
using Commitments.Core;
using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveBehaviourHandlerTests
{
    [Fact]
    public async Task Handle_ExistingBehaviour_RemovesAndSaves()
    {
        // Arrange
        var behaviourId = Guid.NewGuid();
        var existingBehaviour = new Behaviour { BehaviourId = behaviourId };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviours = new List<Behaviour> { existingBehaviour };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviours);
        mockDbSet.Setup(d => d.FindAsync(behaviourId)).ReturnsAsync(existingBehaviour);
        mockContext.Setup(c => c.Behaviours).Returns(mockDbSet.Object);

        var handler = new RemoveBehaviourCommandHandler(mockContext.Object);
        var request = new RemoveBehaviourRequest { BehaviourId = behaviourId };

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        mockDbSet.Verify(d => d.Remove(existingBehaviour), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void RemoveBehaviourRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new RemoveBehaviourRequest { BehaviourId = id };

        request.BehaviourId.Should().Be(id);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new RemoveBehaviourCommandValidator();
        var request = new RemoveBehaviourRequest { BehaviourId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new RemoveBehaviourCommandValidator();
        var request = new RemoveBehaviourRequest { BehaviourId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
