using Identity.Api.Features.Profile;
using Identity.Core;
using Identity.Core.Model.ProfileAggregate;
using Commitments.Shared;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Commands;

public class DeleteProfileHandlerTests
{
    private readonly Mock<IIdentityDbContext> _mockContext;
    private readonly Mock<IEventBus> _mockEventBus;
    private readonly DeleteProfileRequestHandler _handler;

    public DeleteProfileHandlerTests()
    {
        _mockContext = MockIdentityDbContextFactory.Create();
        _mockEventBus = new Mock<IEventBus>();
        _mockEventBus.Setup(e => e.PublishAsync(It.IsAny<ProfileDeletedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _handler = new DeleteProfileRequestHandler(_mockContext.Object, _mockEventBus.Object);
    }

    [Fact]
    public async Task Handle_WhenProfileExists_ShouldRemoveSaveAndPublishEvent()
    {
        var profileId = Guid.NewGuid();
        var existingProfile = new ProfileBuilder().WithProfileId(profileId).Build();

        _mockContext.Setup(c => c.Profiles.FindAsync(profileId))
            .ReturnsAsync(existingProfile);

        var request = new DeleteProfileRequest { ProfileId = profileId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        _mockContext.Verify(c => c.Profiles.Remove(existingProfile), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockEventBus.Verify(e => e.PublishAsync(
            It.Is<ProfileDeletedEvent>(evt => evt.ProfileId == profileId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProfileNotFound_ShouldReturnResponseWithoutRemovingOrPublishing()
    {
        var profileId = Guid.NewGuid();
        _mockContext.Setup(c => c.Profiles.FindAsync(profileId))
            .ReturnsAsync((Profile?)null);

        var request = new DeleteProfileRequest { ProfileId = profileId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        _mockContext.Verify(c => c.Profiles.Remove(It.IsAny<Profile>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockEventBus.Verify(e => e.PublishAsync(It.IsAny<ProfileDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
