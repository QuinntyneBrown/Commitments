using Commitments.Features.Behaviour;
using Commitments.Data;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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

    [Fact]
    public async Task Handle_WhenReferencedByCommitment_ThrowsBadHttpRequestException(/* design 06 Slice C / L2-005 AC#3 */)
    {
        var behaviourId = Guid.NewGuid();
        var existing = new Behaviour { BehaviourId = behaviourId };
        var referencingCommitment = new Commitment { CommitmentId = Guid.NewGuid(), BehaviourId = behaviourId };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviourSet = MockDbSetFactory.CreateMockDbSet(new List<Behaviour> { existing });
        behaviourSet.Setup(d => d.FindAsync(behaviourId)).ReturnsAsync(existing);
        mockContext.Setup(c => c.Behaviours).Returns(behaviourSet.Object);
        mockContext.Setup(c => c.Commitments).Returns(MockDbSetFactory.CreateMockDbSet(new List<Commitment> { referencingCommitment }).Object);

        var handler = new RemoveBehaviourCommandHandler(mockContext.Object);

        var act = async () => await handler.Handle(
            new RemoveBehaviourRequest { BehaviourId = behaviourId },
            CancellationToken.None);

        await act.Should().ThrowAsync<BadHttpRequestException>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete"));

        behaviourSet.Verify(d => d.Remove(It.IsAny<Behaviour>()), Times.Never);
    }
}
