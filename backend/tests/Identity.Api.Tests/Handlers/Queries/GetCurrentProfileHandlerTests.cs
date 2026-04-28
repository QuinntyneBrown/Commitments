using Commitments.Testing.Common;
using FluentAssertions;
using Identity.Domain.ProfileAggregate;
using Identity.Features.Profile;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetCurrentProfileHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFirstProfileForUser_WhenOneExists()
    {
        var userId = Guid.NewGuid();
        var profileId = Guid.NewGuid();

        var profiles = new List<Profile>
        {
            new Profile { ProfileId = profileId, UserId = userId, CreatedOn = new DateTime(2026, 1, 1) }
        };

        var mockContext = MockIdentityDbContextFactory.Create(new List<Identity.Domain.UserAggregate.User>(), profiles);

        var handler = new GetCurrentProfileRequestHandler(mockContext.Object);

        var response = await handler.Handle(
            new GetCurrentProfileRequest { UserId = userId },
            CancellationToken.None);

        response.Profile.Should().NotBeNull();
        response.Profile!.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public async Task Handle_ShouldReturnEarliestProfile_WhenMultipleExist()
    {
        var userId = Guid.NewGuid();
        var earliest = Guid.NewGuid();
        var later = Guid.NewGuid();

        var profiles = new List<Profile>
        {
            new Profile { ProfileId = later, UserId = userId, CreatedOn = new DateTime(2026, 6, 1) },
            new Profile { ProfileId = earliest, UserId = userId, CreatedOn = new DateTime(2026, 1, 1) }
        };

        var mockContext = MockIdentityDbContextFactory.Create(new List<Identity.Domain.UserAggregate.User>(), profiles);

        var handler = new GetCurrentProfileRequestHandler(mockContext.Object);

        var response = await handler.Handle(
            new GetCurrentProfileRequest { UserId = userId },
            CancellationToken.None);

        response.Profile.Should().NotBeNull();
        response.Profile!.ProfileId.Should().Be(earliest);
    }

    [Fact]
    public async Task Handle_ShouldReturnNullProfile_WhenUserHasNone()
    {
        var userId = Guid.NewGuid();
        var otherUser = Guid.NewGuid();

        var profiles = new List<Profile>
        {
            new Profile { ProfileId = Guid.NewGuid(), UserId = otherUser, CreatedOn = DateTime.UtcNow }
        };

        var mockContext = MockIdentityDbContextFactory.Create(new List<Identity.Domain.UserAggregate.User>(), profiles);

        var handler = new GetCurrentProfileRequestHandler(mockContext.Object);

        var response = await handler.Handle(
            new GetCurrentProfileRequest { UserId = userId },
            CancellationToken.None);

        response.Profile.Should().BeNull();
    }
}
