using Commitments.Features.Frequency;
using Commitments.Data;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveFrequencyHandlerTests
{
    [Fact]
    public async Task Handle_NewFrequency_AddsAndSaves()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencies = new List<Frequency>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencies);
        mockContext.Setup(c => c.Frequencies).Returns(mockDbSet.Object);

        var frequencyTypeId = Guid.NewGuid();
        var frequencyValue = Guid.NewGuid();
        var handler = new SaveFrequencyCommandHandler(mockContext.Object);
        var request = new SaveFrequencyRequest
        {
            Frequency = new FrequencyDto
            {
                Frequency = frequencyValue,
                FrequencyTypeId = frequencyTypeId
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        frequencies.Should().HaveCount(1);
        frequencies[0].Frequency.Should().Be(frequencyValue);
        frequencies[0].FrequencyTypeId.Should().Be(frequencyTypeId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingFrequency_UpdatesAndSaves()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var existingFrequency = new Frequency
        {
            FrequencyId = existingId,
            Frequency = Guid.NewGuid(),
            FrequencyTypeId = Guid.NewGuid()
        };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencies = new List<Frequency> { existingFrequency };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencies);
        mockContext.Setup(c => c.Frequencies).Returns(mockDbSet.Object);

        var newFrequencyValue = Guid.NewGuid();
        var newFrequencyTypeId = Guid.NewGuid();
        var handler = new SaveFrequencyCommandHandler(mockContext.Object);
        var request = new SaveFrequencyRequest
        {
            Frequency = new FrequencyDto
            {
                FrequencyId = existingId,
                Frequency = newFrequencyValue,
                FrequencyTypeId = newFrequencyTypeId
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FrequencyId.Should().Be(existingId);
        existingFrequency.Frequency.Should().Be(newFrequencyValue);
        existingFrequency.FrequencyTypeId.Should().Be(newFrequencyTypeId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SaveFrequencyRequest_Properties_CanBeSetAndRetrieved()
    {
        var dto = new FrequencyDto { FrequencyId = Guid.NewGuid() };
        var request = new SaveFrequencyRequest { Frequency = dto };

        request.Frequency.Should().BeSameAs(dto);
    }

    [Fact]
    public void SaveFrequencyResponse_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var response = new SaveFrequencyResponse { FrequencyId = id };

        response.FrequencyId.Should().Be(id);
    }
}
