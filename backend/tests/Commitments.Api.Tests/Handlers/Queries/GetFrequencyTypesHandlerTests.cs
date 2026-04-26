using Commitments.Features.FrequencyType;
using Commitments.Data;
using Commitments.Domain.FrequencyTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetFrequencyTypesHandlerTests
{
    [Fact]
    public async Task Handle_NoFrequencyTypes_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var handler = new GetFrequencyTypesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetFrequencyTypesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FrequencyTypes.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithFrequencyTypes_ReturnsPopulatedList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencyTypes = new List<FrequencyType>
        {
            new FrequencyType { FrequencyTypeId = Guid.NewGuid(), Name = "per day" },
            new FrequencyType { FrequencyTypeId = Guid.NewGuid(), Name = "per week" }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencyTypes);
        mockContext.Setup(c => c.FrequencyTypes).Returns(mockDbSet.Object);

        var handler = new GetFrequencyTypesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetFrequencyTypesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FrequencyTypes.Should().HaveCount(2);
    }

    [Fact]
    public void GetFrequencyTypesResponse_Properties_CanBeSetAndRetrieved()
    {
        var dtos = new List<FrequencyTypeDto> { new FrequencyTypeDto { FrequencyTypeId = Guid.NewGuid() } };
        var response = new GetFrequencyTypesResponse { FrequencyTypes = dtos };

        response.FrequencyTypes.Should().BeSameAs(dtos);
    }
}
