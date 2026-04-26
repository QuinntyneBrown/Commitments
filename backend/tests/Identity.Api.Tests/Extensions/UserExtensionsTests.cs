using Identity.Api.Features.User;
using Identity.Core.Model.UserAggregate;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Xunit;

namespace Identity.Api.Tests.Extensions;

public class UserExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        var userId = Guid.NewGuid();
        var user = new UserBuilder()
            .WithUserId(userId)
            .WithUsername("johndoe")
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithEmail("john@example.com")
            .Build();

        var dto = user.ToDto();

        dto.Should().NotBeNull();
        dto.UserId.Should().Be(userId);
        dto.Username.Should().Be("johndoe");
        dto.FirstName.Should().Be("John");
        dto.LastName.Should().Be("Doe");
        dto.Email.Should().Be("john@example.com");
    }

    [Fact]
    public void ToDto_WithNullOptionalFields_ShouldMapCorrectly()
    {
        var user = new UserBuilder()
            .WithUsername("minimaluser")
            .WithFirstName(null)
            .WithLastName(null)
            .WithEmail(null)
            .Build();

        var dto = user.ToDto();

        dto.Should().NotBeNull();
        dto.Username.Should().Be("minimaluser");
        dto.FirstName.Should().BeNull();
        dto.LastName.Should().BeNull();
        dto.Email.Should().BeNull();
    }
}
