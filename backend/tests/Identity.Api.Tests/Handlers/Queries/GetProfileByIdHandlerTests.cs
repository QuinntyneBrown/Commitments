using Identity.Api.Features.Profile;
using Identity.Core;
using Identity.Core.Model.ProfileAggregate;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Queries;

public class GetProfileByIdHandlerTests
{
    private readonly Mock<IIdentityDbContext> _mockContext;
    private readonly GetProfileByIdRequestHandler _handler;

    public GetProfileByIdHandlerTests()
    {
        _mockContext = MockIdentityDbContextFactory.Create();
        _handler = new GetProfileByIdRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_WhenProfileExists_ShouldReturnProfileDto()
    {
        var profileId = Guid.NewGuid();
        var existingProfile = new ProfileBuilder().WithProfileId(profileId).Build();

        _mockContext.Setup(c => c.Profiles.FindAsync(profileId))
            .ReturnsAsync(existingProfile);

        var request = new GetProfileByIdRequest { ProfileId = profileId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Profile.Should().NotBeNull();
        response.Profile!.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public async Task Handle_WhenProfileNotFound_ShouldReturnNullProfile()
    {
        var profileId = Guid.NewGuid();
        _mockContext.Setup(c => c.Profiles.FindAsync(profileId))
            .ReturnsAsync((Profile?)null);

        var request = new GetProfileByIdRequest { ProfileId = profileId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Profile.Should().BeNull();
    }
}
