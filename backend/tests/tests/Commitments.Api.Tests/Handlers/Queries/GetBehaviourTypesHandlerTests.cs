using Commitments.Api.Features.BehaviourType;
using Commitments.Core;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetBehaviourTypesHandlerTests
{
    [Fact]
    public async Task Handle_NoBehaviourTypes_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var handler = new GetBehaviourTypesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetBehaviourTypesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.BehaviourTypes.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithBehaviourTypes_ReturnsPopulatedList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviourTypes = new List<BehaviourType>
        {
            new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type1" },
            new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type2" }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviourTypes);
        mockContext.Setup(c => c.BehaviourTypes).Returns(mockDbSet.Object);

        var handler = new GetBehaviourTypesQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetBehaviourTypesRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.BehaviourTypes.Should().HaveCount(2);
    }

    [Fact]
    public void GetBehaviourTypesResponse_Properties_CanBeSetAndRetrieved()
    {
        var dtos = new List<BehaviourTypeDto> { new BehaviourTypeDto { BehaviourTypeId = Guid.NewGuid() } };
        var response = new GetBehaviourTypesResponse { BehaviourTypes = dtos };

        response.BehaviourTypes.Should().BeSameAs(dtos);
    }
}
