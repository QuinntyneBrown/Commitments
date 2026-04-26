using Identity.Api.Features.Profile;
using Identity.Core.Model.ProfileAggregate;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Xunit;

namespace Identity.Api.Tests.Extensions;

public class ProfileExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapProfileId()
    {
        var profileId = Guid.NewGuid();
        var profile = new ProfileBuilder().WithProfileId(profileId).Build();

        var dto = profile.ToDto();

        dto.Should().NotBeNull();
        dto.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public void ToDto_ShouldReturnNonNullDto()
    {
        var profile = new ProfileBuilder().Build();

        var dto = profile.ToDto();

        dto.Should().NotBeNull();
        dto.ProfileId.Should().NotBeEmpty();
    }
}
