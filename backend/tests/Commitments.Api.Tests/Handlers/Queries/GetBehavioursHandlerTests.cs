using Commitments.Features.Behaviour;
using Commitments.Data;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetBehavioursHandlerTests
{
    [Fact]
    public async Task Handle_NoBehaviours_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var handler = new GetBehavioursQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetBehavioursRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Behaviours.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithBehaviours_ReturnsPopulatedList()
    {
        // Arrange
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type1" };
        var behaviours = new List<Behaviour>
        {
            new Behaviour
            {
                BehaviourId = Guid.NewGuid(),
                Name = "Behaviour1",
                Description = "Desc1",
                BehaviourTypeId = behaviourType.BehaviourTypeId,
                BehaviourType = behaviourType
            },
            new Behaviour
            {
                BehaviourId = Guid.NewGuid(),
                Name = "Behaviour2",
                Description = "Desc2",
                BehaviourTypeId = behaviourType.BehaviourTypeId,
                BehaviourType = behaviourType
            }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviours);
        mockContext.Setup(c => c.Behaviours).Returns(mockDbSet.Object);

        var handler = new GetBehavioursQueryHandler(mockContext.Object);

        // Act
        var result = await handler.Handle(new GetBehavioursRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Behaviours.Should().HaveCount(2);
    }

    [Fact]
    public void GetBehavioursResponse_Properties_CanBeSetAndRetrieved()
    {
        var dtos = new List<BehaviourDto> { new BehaviourDto { BehaviourId = Guid.NewGuid() } };
        var response = new GetBehavioursResponse { Behaviours = dtos };

        response.Behaviours.Should().BeSameAs(dtos);
    }
}
