using Commitments.Features.BehaviourType;
using Commitments.Data;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetBehaviourTypeByIdHandlerTests
{
    [Fact]
    public async Task Handle_ExistingBehaviourType_ReturnsBehaviourType()
    {
        // Arrange
        var behaviourTypeId = Guid.NewGuid();
        var behaviourType = new BehaviourType
        {
            BehaviourTypeId = behaviourTypeId,
            Name = "Test BehaviourType"
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(new List<BehaviourType> { behaviourType });
        mockDbSet.Setup(d => d.FindAsync(behaviourTypeId)).ReturnsAsync(behaviourType);
        mockContext.Setup(c => c.BehaviourTypes).Returns(mockDbSet.Object);

        var handler = new GetBehaviourTypeByIdHandler(mockContext.Object);
        var request = new GetBehaviourTypeByIdRequest { BehaviourTypeId = behaviourTypeId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.BehaviourType.Should().NotBeNull();
        result.BehaviourType.BehaviourTypeId.Should().Be(behaviourTypeId);
        result.BehaviourType.Name.Should().Be("Test BehaviourType");
    }

    [Fact]
    public void GetBehaviourTypeByIdRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new GetBehaviourTypeByIdRequest { BehaviourTypeId = id };

        request.BehaviourTypeId.Should().Be(id);
    }

    [Fact]
    public void GetBehaviourTypeByIdResponse_Properties_CanBeSetAndRetrieved()
    {
        var dto = new BehaviourTypeDto { BehaviourTypeId = Guid.NewGuid() };
        var response = new GetBehaviourTypeByIdResponse { BehaviourType = dto };

        response.BehaviourType.Should().BeSameAs(dto);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new GetBehaviourTypeByIdValidator();
        var request = new GetBehaviourTypeByIdRequest { BehaviourTypeId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new GetBehaviourTypeByIdValidator();
        var request = new GetBehaviourTypeByIdRequest { BehaviourTypeId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
