using Commitments.Features.BehaviourType;
using Commitments.Data;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveBehaviourTypeHandlerTests
{
    [Fact]
    public async Task Handle_ExistingBehaviourType_RemovesAndSaves()
    {
        // Arrange
        var behaviourTypeId = Guid.NewGuid();
        var existingBehaviourType = new BehaviourType { BehaviourTypeId = behaviourTypeId };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviourTypes = new List<BehaviourType> { existingBehaviourType };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviourTypes);
        mockDbSet.Setup(d => d.FindAsync(behaviourTypeId)).ReturnsAsync(existingBehaviourType);
        mockContext.Setup(c => c.BehaviourTypes).Returns(mockDbSet.Object);

        var handler = new RemoveBehaviourTypeCommandHandler(mockContext.Object);
        var request = new RemoveBehaviourTypeRequest { BehaviourTypeId = behaviourTypeId };

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        mockDbSet.Verify(d => d.Remove(existingBehaviourType), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void RemoveBehaviourTypeRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new RemoveBehaviourTypeRequest { BehaviourTypeId = id };

        request.BehaviourTypeId.Should().Be(id);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new RemoveBehaviourTypeCommandValidator();
        var request = new RemoveBehaviourTypeRequest { BehaviourTypeId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new RemoveBehaviourTypeCommandValidator();
        var request = new RemoveBehaviourTypeRequest { BehaviourTypeId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
