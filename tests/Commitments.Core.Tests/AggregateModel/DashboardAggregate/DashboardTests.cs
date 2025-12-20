// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.DashboardAggregate;
using Commitments.Core.AggregateModel.DashboardCardAggregate;
using Commitments.Core.AggregateModel.UserAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.DashboardAggregate;

public class DashboardTests
{
    [Fact]
    public void Dashboard_ShouldInitializeWithName()
    {
        // Arrange
        var name = "Test Dashboard";

        // Act
        var dashboard = new Dashboard(name);

        // Assert
        dashboard.Name.Should().Be(name);
        dashboard.DashboardCards.Should().NotBeNull();
        dashboard.DashboardCards.Should().BeEmpty();
    }

    [Fact]
    public void Dashboard_ShouldInitializeWithNameAndUserId()
    {
        // Arrange
        var name = "Test Dashboard";
        var userId = Guid.NewGuid();

        // Act
        var dashboard = new Dashboard(name, userId);

        // Assert
        dashboard.Name.Should().Be(name);
        dashboard.UserId.Should().Be(userId);
    }

    [Fact]
    public void Dashboard_ShouldSetDashboardId()
    {
        // Arrange
        var dashboard = new Dashboard("Test");
        var id = Guid.NewGuid();

        // Act
        dashboard.DashboardId = id;

        // Assert
        dashboard.DashboardId.Should().Be(id);
    }

    [Fact]
    public void Dashboard_ShouldSetName()
    {
        // Arrange
        var dashboard = new Dashboard("Initial");
        var newName = "Updated Dashboard";

        // Act
        dashboard.Name = newName;

        // Assert
        dashboard.Name.Should().Be(newName);
    }

    [Fact]
    public void Dashboard_ShouldSetUserId()
    {
        // Arrange
        var dashboard = new Dashboard("Test");
        var userId = Guid.NewGuid();

        // Act
        dashboard.UserId = userId;

        // Assert
        dashboard.UserId.Should().Be(userId);
    }

    [Fact]
    public void Dashboard_ShouldSetProfileId()
    {
        // Arrange
        var dashboard = new Dashboard("Test");
        var profileId = Guid.NewGuid();

        // Act
        dashboard.ProfileId = profileId;

        // Assert
        dashboard.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public void Dashboard_ShouldAllowNullUserId()
    {
        // Arrange
        var dashboard = new Dashboard("Test", Guid.NewGuid());

        // Act
        dashboard.UserId = null;

        // Assert
        dashboard.UserId.Should().BeNull();
    }

    [Fact]
    public void Dashboard_ShouldAllowNullProfileId()
    {
        // Arrange
        var dashboard = new Dashboard("Test");

        // Act
        dashboard.ProfileId = null;

        // Assert
        dashboard.ProfileId.Should().BeNull();
    }

    [Fact]
    public void Dashboard_ShouldSetUser()
    {
        // Arrange
        var dashboard = new Dashboard("Test");
        var user = new User { UserId = Guid.NewGuid(), Username = "testuser" };

        // Act
        dashboard.User = user;

        // Assert
        dashboard.User.Should().Be(user);
    }

    [Fact]
    public void Dashboard_ShouldAddDashboardCard()
    {
        // Arrange
        var dashboard = new Dashboard("Test");
        var dashboardCard = new DashboardCard { DashboardCardId = Guid.NewGuid() };

        // Act
        dashboard.DashboardCards.Add(dashboardCard);

        // Assert
        dashboard.DashboardCards.Should().HaveCount(1);
        dashboard.DashboardCards.Should().Contain(dashboardCard);
    }
}
