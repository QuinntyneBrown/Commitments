using Commitments.Testing.Common;
using FluentAssertions;
using Identity.Domain.ProfileAggregate;
using Identity.Features.Profile;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Identity.Api.Tests.Handlers.Commands;

public class SaveAvatarUrlHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateAvatarUrlOnTargetProfile()
    {
        var profileId = Guid.NewGuid();
        var profile = new Profile { ProfileId = profileId, AvatarUrl = "old.png" };

        var options = new DbContextOptionsBuilder<Identity.Data.IdentityDbContext>()
            .UseInMemoryDatabase($"avatar-{Guid.NewGuid()}")
            .Options;

        await using var ctx = new Identity.Data.IdentityDbContext(options);
        ctx.Profiles.Add(profile);
        await ctx.SaveChangesAsync(CancellationToken.None);

        var handler = new SaveAvatarUrlRequestHandler(ctx);

        var response = await handler.Handle(
            new SaveAvatarUrlRequest { ProfileId = profileId, AvatarUrl = "new.png" },
            CancellationToken.None);

        response.ProfileId.Should().Be(profileId);
        var saved = await ctx.Profiles.SingleAsync();
        saved.AvatarUrl.Should().Be("new.png");
    }
}
