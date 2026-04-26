using Commitments.Api.Features.FrequencyType;
using Commitments.Core;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveFrequencyTypeHandlerTests
{
    [Fact]
    public async Task Handle_NewFrequencyType_AddsAndSaves()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencyTypes = new List<FrequencyType>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencyTypes);
        mockContext.Setup(c => c.FrequencyTypes).Returns(mockDbSet.Object);

        var handler = new SaveFrequencyTypeCommandHandler(mockContext.Object);
        var request = new SaveFrequencyTypeRequest
        {
            FrequencyType = new FrequencyTypeDto
            {
                Name = "Test FrequencyType"
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        frequencyTypes.Should().HaveCount(1);
        frequencyTypes[0].Name.Should().Be("Test FrequencyType");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingFrequencyType_UpdatesAndSaves()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var existingFrequencyType = new FrequencyType
        {
            FrequencyTypeId = existingId,
            Name = "Old Name"
        };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencyTypes = new List<FrequencyType> { existingFrequencyType };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencyTypes);
        mockDbSet.Setup(d => d.FindAsync(existingId)).ReturnsAsync(existingFrequencyType);
        mockContext.Setup(c => c.FrequencyTypes).Returns(mockDbSet.Object);

        var handler = new SaveFrequencyTypeCommandHandler(mockContext.Object);
        var request = new SaveFrequencyTypeRequest
        {
            FrequencyType = new FrequencyTypeDto
            {
                FrequencyTypeId = existingId,
                Name = "Updated Name"
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FrequencyTypeId.Should().Be(existingId);
        existingFrequencyType.Name.Should().Be("Updated Name");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SaveFrequencyTypeRequest_Properties_CanBeSetAndRetrieved()
    {
        var dto = new FrequencyTypeDto { FrequencyTypeId = Guid.NewGuid(), Name = "Test" };
        var request = new SaveFrequencyTypeRequest { FrequencyType = dto };

        request.FrequencyType.Should().BeSameAs(dto);
    }

    [Fact]
    public void SaveFrequencyTypeResponse_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var response = new SaveFrequencyTypeResponse { FrequencyTypeId = id };

        response.FrequencyTypeId.Should().Be(id);
    }
}
