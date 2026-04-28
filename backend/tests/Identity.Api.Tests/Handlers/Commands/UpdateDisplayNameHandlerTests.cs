using FluentAssertions;
using Identity.Domain.ProfileAggregate;
using Identity.Features.Profile;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Identity.Api.Tests.Handlers.Commands;

public class UpdateDisplayNameHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateDisplayNameOnTargetProfile()
    {
        var profileId = Guid.NewGuid();
        var profile = new Profile { ProfileId = profileId, DisplayName = "Old" };

        var options = new DbContextOptionsBuilder<Identity.Data.IdentityDbContext>()
            .UseInMemoryDatabase($"display-{Guid.NewGuid()}")
            .Options;

        await using var ctx = new Identity.Data.IdentityDbContext(options);
        ctx.Profiles.Add(profile);
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new UpdateDisplayNameRequestHandler(ctx);

        var response = await handler.Handle(
            new UpdateDisplayNameRequest { ProfileId = profileId, DisplayName = "New Name" },
            CancellationToken.None);

        response.ProfileId.Should().Be(profileId);
        var saved = await ctx.Profiles.SingleAsync();
        saved.DisplayName.Should().Be("New Name");
    }
}
