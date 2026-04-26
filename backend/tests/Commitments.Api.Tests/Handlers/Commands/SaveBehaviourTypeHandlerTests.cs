using Commitments.Features.BehaviourType;
using Commitments.Data;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class SaveBehaviourTypeHandlerTests
{
    [Fact]
    public async Task Handle_NewBehaviourType_AddsAndSaves()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviourTypes = new List<BehaviourType>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviourTypes);
        mockContext.Setup(c => c.BehaviourTypes).Returns(mockDbSet.Object);

        var handler = new SaveBehaviourTypeCommandHandler(mockContext.Object);
        var request = new SaveBehaviourTypeRequest
        {
            BehaviourType = new BehaviourTypeDto
            {
                Name = "Test BehaviourType"
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        behaviourTypes.Should().HaveCount(1);
        behaviourTypes[0].Name.Should().Be("Test BehaviourType");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingBehaviourType_UpdatesAndSaves()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var existingBehaviourType = new BehaviourType
        {
            BehaviourTypeId = existingId,
            Name = "Old Name"
        };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviourTypes = new List<BehaviourType> { existingBehaviourType };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviourTypes);
        mockDbSet.Setup(d => d.FindAsync(existingId)).ReturnsAsync(existingBehaviourType);
        mockContext.Setup(c => c.BehaviourTypes).Returns(mockDbSet.Object);

        var handler = new SaveBehaviourTypeCommandHandler(mockContext.Object);
        var request = new SaveBehaviourTypeRequest
        {
            BehaviourType = new BehaviourTypeDto
            {
                BehaviourTypeId = existingId,
                Name = "Updated Name"
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.BehaviourTypeId.Should().Be(existingId);
        existingBehaviourType.Name.Should().Be("Updated Name");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void SaveBehaviourTypeRequest_Properties_CanBeSetAndRetrieved()
    {
        var dto = new BehaviourTypeDto { BehaviourTypeId = Guid.NewGuid(), Name = "Test" };
        var request = new SaveBehaviourTypeRequest { BehaviourType = dto };

        request.BehaviourType.Should().BeSameAs(dto);
    }

    [Fact]
    public void SaveBehaviourTypeResponse_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var response = new SaveBehaviourTypeResponse { BehaviourTypeId = id };

        response.BehaviourTypeId.Should().Be(id);
    }
}
