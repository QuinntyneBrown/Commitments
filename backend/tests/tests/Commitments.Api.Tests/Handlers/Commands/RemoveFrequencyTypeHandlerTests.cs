using Commitments.Api.Features.FrequencyType;
using Commitments.Core;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Handlers.Commands;

public class RemoveFrequencyTypeHandlerTests
{
    [Fact]
    public async Task Handle_ExistingFrequencyType_RemovesAndSaves()
    {
        // Arrange
        var frequencyTypeId = Guid.NewGuid();
        var existingFrequencyType = new FrequencyType { FrequencyTypeId = frequencyTypeId };
        var mockContext = MockCommitmentsDbContextFactory.Create();
        var frequencyTypes = new List<FrequencyType> { existingFrequencyType };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(frequencyTypes);
        mockDbSet.Setup(d => d.FindAsync(frequencyTypeId)).ReturnsAsync(existingFrequencyType);
        mockContext.Setup(c => c.FrequencyTypes).Returns(mockDbSet.Object);

        var handler = new RemoveFrequencyTypeCommandHandler(mockContext.Object);
        var request = new RemoveFrequencyTypeRequest { FrequencyTypeId = frequencyTypeId };

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        mockDbSet.Verify(d => d.Remove(existingFrequencyType), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void RemoveFrequencyTypeRequest_Properties_CanBeSetAndRetrieved()
    {
        var id = Guid.NewGuid();
        var request = new RemoveFrequencyTypeRequest { FrequencyTypeId = id };

        request.FrequencyTypeId.Should().Be(id);
    }

    [Fact]
    public void Validate_ValidGuid_Passes()
    {
        var validator = new RemoveFrequencyTypeCommandValidator();
        var request = new RemoveFrequencyTypeRequest { FrequencyTypeId = Guid.NewGuid() };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DefaultGuid_Fails()
    {
        var validator = new RemoveFrequencyTypeCommandValidator();
        var request = new RemoveFrequencyTypeRequest { FrequencyTypeId = default(Guid) };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
