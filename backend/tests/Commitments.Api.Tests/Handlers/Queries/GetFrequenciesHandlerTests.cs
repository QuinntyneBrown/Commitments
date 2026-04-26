using Commitments.Api.Features.Frequency;
using Commitments.Core;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetFrequenciesHandlerTests
{
    [Fact]
    public async Task Handle_NoFrequencies_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var handler = new GetFrequenciesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetFrequenciesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Frequencies.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithFrequencies_ReturnsPopulatedList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencyType = new FrequencyType { FrequencyTypeId = Guid.NewGuid(), Name = "per day" };
        var frequencies = new List<Frequency>
        {
            new Frequency
            {
                FrequencyId = Guid.NewGuid(),
                Frequency = Guid.NewGuid(),
                FrequencyTypeId = frequencyType.FrequencyTypeId,
                FrequencyType = frequencyType
            },
            new Frequency
            {
                FrequencyId = Guid.NewGuid(),
                Frequency = Guid.NewGuid(),
                FrequencyTypeId = frequencyType.FrequencyTypeId,
                FrequencyType = frequencyType
            }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencies);
        mockContext.Setup(c => c.Frequencies).Returns(mockDbSet.Object);

        var handler = new GetFrequenciesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetFrequenciesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Frequencies.Should().HaveCount(2);
    }

    [Fact]
    public void GetFrequenciesResponse_Properties_CanBeSetAndRetrieved()
    {
        var dtos = new List<FrequencyDto> { new FrequencyDto { FrequencyId = Guid.NewGuid() } };
        var response = new GetFrequenciesResponse { Frequencies = dtos };

        response.Frequencies.Should().BeSameAs(dtos);
    }
}
