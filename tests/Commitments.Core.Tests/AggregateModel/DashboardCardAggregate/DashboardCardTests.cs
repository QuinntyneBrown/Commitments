// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CardAggregate;
using Commitments.Core.AggregateModel.CardLayoutAggregate;
using Commitments.Core.AggregateModel.DashboardAggregate;
using Commitments.Core.AggregateModel.DashboardCardAggregate;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.DashboardCardAggregate;

public class DashboardCardTests
{
    [Fact]
    public void DashboardCard_ShouldSetDashboardCardId()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var id = Guid.NewGuid();

        // Act
        dashboardCard.DashboardCardId = id;

        // Assert
        dashboardCard.DashboardCardId.Should().Be(id);
    }

    [Fact]
    public void DashboardCard_ShouldSetDashboardId()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var dashboardId = Guid.NewGuid();

        // Act
        dashboardCard.DashboardId = dashboardId;

        // Assert
        dashboardCard.DashboardId.Should().Be(dashboardId);
    }

    [Fact]
    public void DashboardCard_ShouldSetCardId()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var cardId = Guid.NewGuid();

        // Act
        dashboardCard.CardId = cardId;

        // Assert
        dashboardCard.CardId.Should().Be(cardId);
    }

    [Fact]
    public void DashboardCard_ShouldSetCardLayoutId()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var cardLayoutId = Guid.NewGuid();

        // Act
        dashboardCard.CardLayoutId = cardLayoutId;

        // Assert
        dashboardCard.CardLayoutId.Should().Be(cardLayoutId);
    }

    [Fact]
    public void DashboardCard_ShouldSetDashboard()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var dashboard = new Dashboard("Test Dashboard");

        // Act
        dashboardCard.Dashboard = dashboard;

        // Assert
        dashboardCard.Dashboard.Should().Be(dashboard);
    }

    [Fact]
    public void DashboardCard_ShouldSetCard()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var card = new Card { CardId = Guid.NewGuid(), Name = "Test Card" };

        // Act
        dashboardCard.Card = card;

        // Assert
        dashboardCard.Card.Should().Be(card);
    }

    [Fact]
    public void DashboardCard_ShouldSetCardLayout()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var cardLayout = new CardLayout { CardLayoutId = Guid.NewGuid() };

        // Act
        dashboardCard.CardLayout = cardLayout;

        // Assert
        dashboardCard.CardLayout.Should().Be(cardLayout);
    }

    [Fact]
    public void DashboardCard_ShouldSetOptions()
    {
        // Arrange
        var dashboardCard = new DashboardCard();
        var options = JObject.Parse("{\"key\": \"value\"}");

        // Act
        dashboardCard.Options = options;

        // Assert
        dashboardCard.Options.Should().NotBeNull();
        dashboardCard.Options["key"]!.Value<string>().Should().Be("value");
    }
}
