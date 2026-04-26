using Commitments.Api.Features.Frequency;
using Commitments.Core;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Queries;

public class GetFrequencyByIdHandlerTests
{
    [Fact]
    public async Task Handle_ExistingFrequency_ReturnsFrequency()
    {
        // Arrange
        var frequencyId = Guid.NewGuid();
        var frequencyType = new FrequencyType { FrequencyTypeId = Guid.NewGuid(), Name = "per day" };
        var frequency = new Frequency
        {
            FrequencyId = frequencyId,
            Frequency = Guid.NewGuid(),
            FrequencyTypeId = frequencyType.FrequencyTypeId,
            FrequencyType = frequencyType
        };

        var mockContext = MockCommitmentsDbContextFactory.Create();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(new List<Frequency> { frequency });
        mockDbSet.Setup(d => d.FindAsync(frequencyId)).ReturnsAsync(frequency);
        mockContext.Setup(c => c.Frequencies).Returns(mockDbSet.Object);

        var handler = new GetFrequencyByIdHandler(mockContext.Object);
        var request = new GetFrequencyByIdRequest { FrequencyId = frequencyId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Frequency.Should().NotBeNull();
        result.Frequency.FrequencyId.Should().Be(frequencyId);
    }

    [Fact]
    public void GetFrequencyByIdRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new GetFrequencyByIdRequest { FrequencyId = id };

        request.FrequencyId.Should().Be(id);
    }

    [Fact]
    public void GetFrequencyByIdResponse_Properties_CanBeSetAndRetrieved()
    {
        var dto = new FrequencyDto { FrequencyId = Guid.NewGuid() };
        var response = new GetFrequencyByIdResponse { Frequency = dto };

        response.Frequency.Should().BeSameAs(dto);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new GetFrequencyByIdValidator();
        var request = new GetFrequencyByIdRequest { FrequencyId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new GetFrequencyByIdValidator();
        var request = new GetFrequencyByIdRequest { FrequencyId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
