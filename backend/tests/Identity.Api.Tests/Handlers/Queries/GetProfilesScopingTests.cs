using Commitments.Testing.Common;
using FluentAssertions;
using Identity.Domain.ProfileAggregate;
using Identity.Features.Profile;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetProfilesScopingTests
{
    [Fact]
    public async Task Handle_ShouldReturnOnlyProfilesForRequestedUser()
    {
        var caller = Guid.NewGuid();
        var other = Guid.NewGuid();

        var profiles = new List<Profile>
        {
            new Profile { ProfileId = Guid.NewGuid(), UserId = caller },
            new Profile { ProfileId = Guid.NewGuid(), UserId = caller },
            new Profile { ProfileId = Guid.NewGuid(), UserId = other }
        };

        var mockContext = MockIdentityDbContextFactory.Create(new List<Identity.Domain.UserAggregate.User>(), profiles);
        var handler = new GetProfilesRequestHandler(mockContext.Object);

        var response = await handler.Handle(
            new GetProfilesRequest { UserId = caller },
            CancellationToken.None);

        response.Profiles.Should().HaveCount(2);
        response.Profiles.Should().OnlyContain(p => profiles.Any(src => src.ProfileId == p.ProfileId && src.UserId == caller));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenCallerHasNoProfiles()
    {
        var caller = Guid.NewGuid();
        var other = Guid.NewGuid();

        var profiles = new List<Profile>
        {
            new Profile { ProfileId = Guid.NewGuid(), UserId = other }
        };

        var mockContext = MockIdentityDbContextFactory.Create(new List<Identity.Domain.UserAggregate.User>(), profiles);
        var handler = new GetProfilesRequestHandler(mockContext.Object);

        var response = await handler.Handle(
            new GetProfilesRequest { UserId = caller },
            CancellationToken.None);

        response.Profiles.Should().BeEmpty();
    }
}
