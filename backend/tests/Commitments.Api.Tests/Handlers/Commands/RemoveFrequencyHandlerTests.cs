using Commitments.Features.Frequency;
using Commitments.Data;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveFrequencyHandlerTests
{
    [Fact]
    public async Task Handle_ExistingFrequency_RemovesAndSaves()
    {
        // Arrange
        var frequencyId = Guid.NewGuid();
        var existingFrequency = new Frequency { FrequencyId = frequencyId };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencies = new List<Frequency> { existingFrequency };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencies);
        mockDbSet.Setup(d => d.FindAsync(frequencyId)).ReturnsAsync(existingFrequency);
        mockContext.Setup(c => c.Frequencies).Returns(mockDbSet.Object);

        var handler = new RemoveFrequencyCommandHandler(mockContext.Object);
        var request = new RemoveFrequencyRequest { FrequencyId = frequencyId };

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        mockDbSet.Verify(d => d.Remove(existingFrequency), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void RemoveFrequencyRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new RemoveFrequencyRequest { FrequencyId = id };

        request.FrequencyId.Should().Be(id);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new RemoveFrequencyCommandValidator();
        var request = new RemoveFrequencyRequest { FrequencyId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new RemoveFrequencyCommandValidator();
        var request = new RemoveFrequencyRequest { FrequencyId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenReferencedByCommitmentFrequency_ThrowsBadHttpRequestException(/* design 07 Slice D */)
    {
        var frequencyId = Guid.NewGuid();
        var existing = new Frequency { FrequencyId = frequencyId };
        var join = new CommitmentFrequency { CommitmentFrequencyId = Guid.NewGuid(), FrequencyId = frequencyId };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var freqSet = MockDbSetFactory.CreateMockDbSet(new List<Frequency> { existing });
        freqSet.Setup(d => d.FindAsync(frequencyId)).ReturnsAsync(existing);
        mockContext.Setup(c => c.Frequencies).Returns(freqSet.Object);
        mockContext.Setup(c => c.CommitmentFrequencies).Returns(MockDbSetFactory.CreateMockDbSet(new List<CommitmentFrequency> { join }).Object);

        var handler = new RemoveFrequencyCommandHandler(mockContext.Object);

        var act = async () => await handler.Handle(
            new RemoveFrequencyRequest { FrequencyId = frequencyId },
            CancellationToken.None);

        await act.Should().ThrowAsync<BadHttpRequestException>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete"));

        freqSet.Verify(d => d.Remove(It.IsAny<Frequency>()), Times.Never);
    }
}
