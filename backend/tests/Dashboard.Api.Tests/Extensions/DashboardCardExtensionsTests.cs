using Dashboard.Features.DashboardCard;
using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.CardLayoutAggregate;
using Dashboard.Domain.DashboardCardAggregate;
using FluentAssertions;
using Xunit;

namespace Dashboard.Api.Tests.Extensions;

public class DashboardCardExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var dashboardId = Guid.NewGuid();
        var cardId = Guid.NewGuid();
        var cardLayoutId = Guid.NewGuid();
        var card = new Card { CardId = cardId, Name = "Test Card", Description = "Card Desc" };
        var cardLayout = new CardLayout("Test Layout", "Layout Desc") { CardLayoutId = cardLayoutId };

        var dashboardCard = new DashboardCard
        {
            DashboardCardId = dashboardCardId,
            DashboardId = dashboardId,
            CardId = cardId,
            CardLayoutId = cardLayoutId,
            Card = card,
            CardLayout = cardLayout
        };

        // Act
        var dto = dashboardCard.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.DashboardCardId.Should().Be(dashboardCardId);
        dto.DashboardId.Should().Be(dashboardId);
        dto.CardId.Should().Be(cardId);
        dto.CardLayoutId.Should().Be(cardLayoutId);
        dto.Card.Should().NotBeNull();
        dto.Card.CardId.Should().Be(cardId);
        dto.Card.Name.Should().Be("Test Card");
        dto.CardLayout.Should().NotBeNull();
        dto.CardLayout.CardLayoutId.Should().Be(cardLayoutId);
        dto.CardLayout.Name.Should().Be("Test Layout");
    }

    [Fact]
    public void ToDto_WithNullNavigationProperties_ShouldMapCorrectly()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var dashboardCard = new DashboardCard
        {
            DashboardCardId = dashboardCardId,
            DashboardId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            CardLayoutId = Guid.NewGuid(),
            Card = null!,
            CardLayout = null!
        };

        // Act
        var dto = dashboardCard.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.DashboardCardId.Should().Be(dashboardCardId);
        dto.Card.Should().BeNull();
        dto.CardLayout.Should().BeNull();
    }

    [Fact]
    public void ToDto_ShouldReturnNewDtoInstance()
    {
        // Arrange
        var dashboardCard = new DashboardCard
        {
            DashboardCardId = Guid.NewGuid(),
            DashboardId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            CardLayoutId = Guid.NewGuid()
        };

        // Act
        var dto1 = dashboardCard.ToDto();
        var dto2 = dashboardCard.ToDto();

        // Assert
        dto1.Should().NotBeSameAs(dto2);
        dto1.DashboardCardId.Should().Be(dto2.DashboardCardId);
    }
}
