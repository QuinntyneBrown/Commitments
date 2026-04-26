using Commitments.Features.Behaviour;
using Commitments.Data;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetBehaviourByIdHandlerTests
{
    [Fact]
    public async Task Handle_ExistingBehaviour_ReturnsBehaviour()
    {
        // Arrange
        var behaviourId = Guid.NewGuid();
        var behaviourType = new BehaviourType { BehaviourTypeId = Guid.NewGuid(), Name = "Type1" };
        var behaviour = new Behaviour
        {
            BehaviourId = behaviourId,
            Name = "Test Behaviour",
            Description = "Test Description",
            BehaviourTypeId = behaviourType.BehaviourTypeId,
            BehaviourType = behaviourType
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var behaviours = new List<Behaviour> { behaviour };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(behaviours);
        mockContext.Setup(c => c.Behaviours).Returns(mockDbSet.Object);

        var handler = new GetBehaviourByIdHandler(mockContext.Object);
        var request = new GetBehaviourByIdRequest { BehaviourId = behaviourId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Behaviour.Should().NotBeNull();
        result.Behaviour.BehaviourId.Should().Be(behaviourId);
        result.Behaviour.Name.Should().Be("Test Behaviour");
    }

    [Fact]
    public void GetBehaviourByIdRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new GetBehaviourByIdRequest { BehaviourId = id };

        request.BehaviourId.Should().Be(id);
    }

    [Fact]
    public void GetBehaviourByIdResponse_Properties_CanBeSetAndRetrieved()
    {
        var dto = new BehaviourDto { BehaviourId = Guid.NewGuid() };
        var response = new GetBehaviourByIdResponse { Behaviour = dto };

        response.Behaviour.Should().BeSameAs(dto);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new GetBehaviourByIdValidator();
        var request = new GetBehaviourByIdRequest { BehaviourId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new GetBehaviourByIdValidator();
        var request = new GetBehaviourByIdRequest { BehaviourId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
