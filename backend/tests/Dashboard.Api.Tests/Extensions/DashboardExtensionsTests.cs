using Dashboard.Features.Dashboard;
using Dashboard.Domain.DashboardAggregate;
using FluentAssertions;
using Xunit;

namespace Dashboard.Api.Tests.Extensions;

public class DashboardExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var entity = new DashboardEntity("Test Dashboard", profileId) { DashboardId = dashboardId };

        // Act
        var dto = entity.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.DashboardId.Should().Be(dashboardId);
        dto.Name.Should().Be("Test Dashboard");
        dto.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public void ToDto_WithNullProfileId_ShouldMapCorrectly()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var entity = new DashboardEntity("No Profile") { DashboardId = dashboardId };

        // Act
        var dto = entity.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.DashboardId.Should().Be(dashboardId);
        dto.Name.Should().Be("No Profile");
        dto.ProfileId.Should().BeNull();
    }

    [Fact]
    public void ToDto_ShouldReturnNewDtoInstance()
    {
        // Arrange
        var entity = new DashboardEntity("Test", Guid.NewGuid()) { DashboardId = Guid.NewGuid() };

        // Act
        var dto1 = entity.ToDto();
        var dto2 = entity.ToDto();

        // Assert
        dto1.Should().NotBeSameAs(dto2);
        dto1.DashboardId.Should().Be(dto2.DashboardId);
    }
}
