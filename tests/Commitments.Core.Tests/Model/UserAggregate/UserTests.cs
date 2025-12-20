// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DashboardAggregate;
using Commitments.Core.Model.UserAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.UserAggregate;

public class UserTests
{
    [Fact]
    public void User_ShouldInitializeWithEmptyDashboardsCollection()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Dashboards.Should().NotBeNull();
        user.Dashboards.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldSetUserId()
    {
        // Arrange
        var user = new User();
        var id = Guid.NewGuid();

        // Act
        user.UserId = id;

        // Assert
        user.UserId.Should().Be(id);
    }

    [Fact]
    public void User_ShouldSetUsername()
    {
        // Arrange
        var user = new User();
        var username = "testuser";

        // Act
        user.Username = username;

        // Assert
        user.Username.Should().Be(username);
    }

    [Fact]
    public void User_ShouldSetFirstName()
    {
        // Arrange
        var user = new User();
        var firstName = "John";

        // Act
        user.FirstName = firstName;

        // Assert
        user.FirstName.Should().Be(firstName);
    }

    [Fact]
    public void User_ShouldSetLastName()
    {
        // Arrange
        var user = new User();
        var lastName = "Doe";

        // Act
        user.LastName = lastName;

        // Assert
        user.LastName.Should().Be(lastName);
    }

    [Fact]
    public void User_ShouldSetEmail()
    {
        // Arrange
        var user = new User();
        var email = "john.doe@example.com";

        // Act
        user.Email = email;

        // Assert
        user.Email.Should().Be(email);
    }

    [Fact]
    public void User_ShouldAllowNullFirstName()
    {
        // Arrange
        var user = new User();

        // Act
        user.FirstName = null;

        // Assert
        user.FirstName.Should().BeNull();
    }

    [Fact]
    public void User_ShouldAllowNullLastName()
    {
        // Arrange
        var user = new User();

        // Act
        user.LastName = null;

        // Assert
        user.LastName.Should().BeNull();
    }

    [Fact]
    public void User_ShouldAllowNullEmail()
    {
        // Arrange
        var user = new User();

        // Act
        user.Email = null;

        // Assert
        user.Email.Should().BeNull();
    }

    [Fact]
    public void User_ShouldAddDashboard()
    {
        // Arrange
        var user = new User();
        var dashboard = new Dashboard("Test Dashboard");

        // Act
        user.Dashboards.Add(dashboard);

        // Assert
        user.Dashboards.Should().HaveCount(1);
        user.Dashboards.Should().Contain(dashboard);
    }

    [Fact]
    public void User_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var user = new User();
        var now = DateTime.UtcNow;

        user.CreatedOn = now;
        user.LastModifiedOn = now;
        user.IsDeleted = true;

        // Assert
        user.CreatedOn.Should().Be(now);
        user.LastModifiedOn.Should().Be(now);
        user.IsDeleted.Should().BeTrue();
    }
}
