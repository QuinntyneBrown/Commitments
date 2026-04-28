using Commitments.Features.BehaviourType;
using Commitments.Data;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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

    [Fact]
    public async Task Handle_WhenReferencedByBehaviour_ThrowsBadHttpRequestException(/* design 05 Slice B / L2-006 */)
    {
        var behaviourTypeId = Guid.NewGuid();
        var existing = new BehaviourType { BehaviourTypeId = behaviourTypeId };
        var referencingBehaviour = new Behaviour { BehaviourId = Guid.NewGuid(), BehaviourTypeId = behaviourTypeId };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var typeDbSet = MockDbSetFactory.CreateMockDbSet(new List<BehaviourType> { existing });
        typeDbSet.Setup(d => d.FindAsync(behaviourTypeId)).ReturnsAsync(existing);
        mockContext.Setup(c => c.BehaviourTypes).Returns(typeDbSet.Object);
        mockContext.Setup(c => c.Behaviours).Returns(MockDbSetFactory.CreateMockDbSet(new List<Behaviour> { referencingBehaviour }).Object);

        var handler = new RemoveBehaviourTypeCommandHandler(mockContext.Object);

        var act = async () => await handler.Handle(
            new RemoveBehaviourTypeRequest { BehaviourTypeId = behaviourTypeId },
            CancellationToken.None);

        await act.Should().ThrowAsync<BadHttpRequestException>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete"));

        typeDbSet.Verify(d => d.Remove(It.IsAny<BehaviourType>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNotReferenced_RemovesAndSaves()
    {
        var behaviourTypeId = Guid.NewGuid();
        var existing = new BehaviourType { BehaviourTypeId = behaviourTypeId };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var typeDbSet = MockDbSetFactory.CreateMockDbSet(new List<BehaviourType> { existing });
        typeDbSet.Setup(d => d.FindAsync(behaviourTypeId)).ReturnsAsync(existing);
        mockContext.Setup(c => c.BehaviourTypes).Returns(typeDbSet.Object);
        mockContext.Setup(c => c.Behaviours).Returns(MockDbSetFactory.CreateMockDbSet(new List<Behaviour>()).Object);

        var handler = new RemoveBehaviourTypeCommandHandler(mockContext.Object);

        await handler.Handle(new RemoveBehaviourTypeRequest { BehaviourTypeId = behaviourTypeId }, CancellationToken.None);

        typeDbSet.Verify(d => d.Remove(existing), Times.Once);
    }
}
