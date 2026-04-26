using Dashboard.Api.Features.CardLayout;
using FluentAssertions;
using Xunit;
using CardLayoutModel = Dashboard.Core.Model.CardLayoutAggregate.CardLayout;

namespace Dashboard.Api.Tests.Extensions;

public class CardLayoutExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var cardLayout = new CardLayoutModel("Test Layout", "Test Description")
        {
            CardLayoutId = cardLayoutId
        };

        // Act
        var dto = cardLayout.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.CardLayoutId.Should().Be(cardLayoutId);
        dto.Name.Should().Be("Test Layout");
        dto.Description.Should().Be("Test Description");
    }

    [Fact]
    public void ToDto_ShouldReturnNewDtoInstance()
    {
        // Arrange
        var cardLayout = new CardLayoutModel("Layout", "Desc")
        {
            CardLayoutId = Guid.NewGuid()
        };

        // Act
        var dto1 = cardLayout.ToDto();
        var dto2 = cardLayout.ToDto();

        // Assert
        dto1.Should().NotBeSameAs(dto2);
        dto1.CardLayoutId.Should().Be(dto2.CardLayoutId);
        dto1.Name.Should().Be(dto2.Name);
    }

    [Fact]
    public void ToDto_WithEmptyValues_ShouldMapCorrectly()
    {
        // Arrange
        var cardLayout = new CardLayoutModel("", "")
        {
            CardLayoutId = Guid.Empty
        };

        // Act
        var dto = cardLayout.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.CardLayoutId.Should().Be(Guid.Empty);
        dto.Name.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
    }
}
