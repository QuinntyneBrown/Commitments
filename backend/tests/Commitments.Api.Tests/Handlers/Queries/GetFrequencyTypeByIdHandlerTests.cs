using Commitments.Api.Features.FrequencyType;
using Commitments.Core;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetFrequencyTypeByIdHandlerTests
{
    [Fact]
    public async Task Handle_ExistingFrequencyType_ReturnsFrequencyType()
    {
        // Arrange
        var frequencyTypeId = Guid.NewGuid();
        var frequencyType = new FrequencyType
        {
            FrequencyTypeId = frequencyTypeId,
            Name = "per day"
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(new List<FrequencyType> { frequencyType });
        mockDbSet.Setup(d => d.FindAsync(frequencyTypeId)).ReturnsAsync(frequencyType);
        mockContext.Setup(c => c.FrequencyTypes).Returns(mockDbSet.Object);

        var handler = new GetFrequencyTypeByIdHandler(mockContext.Object);
        var request = new GetFrequencyTypeByIdRequest { FrequencyTypeId = frequencyTypeId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FrequencyType.Should().NotBeNull();
        result.FrequencyType.FrequencyTypeId.Should().Be(frequencyTypeId);
        result.FrequencyType.Name.Should().Be("per day");
    }

    [Fact]
    public void GetFrequencyTypeByIdRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new GetFrequencyTypeByIdRequest { FrequencyTypeId = id };

        request.FrequencyTypeId.Should().Be(id);
    }

    [Fact]
    public void GetFrequencyTypeByIdResponse_Properties_CanBeSetAndRetrieved()
    {
        var dto = new FrequencyTypeDto { FrequencyTypeId = Guid.NewGuid() };
        var response = new GetFrequencyTypeByIdResponse { FrequencyType = dto };

        response.FrequencyType.Should().BeSameAs(dto);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new GetFrequencyTypeByIdValidator();
        var request = new GetFrequencyTypeByIdRequest { FrequencyTypeId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new GetFrequencyTypeByIdValidator();
        var request = new GetFrequencyTypeByIdRequest { FrequencyTypeId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
