using Identity.Api.Features.Profile;
using Identity.Core;
using Identity.Core.Model.ProfileAggregate;
using Commitments.Shared;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace Identity.Api.Tests.Handlers.Commands;

public class CreateProfileHandlerTests
{
    private readonly Mock<IIdentityDbContext> _mockContext;
    private readonly Mock<IEventBus> _mockEventBus;
    private readonly CreateProfileRequestHandler _handler;

    public CreateProfileHandlerTests()
    {
        _mockContext = MockIdentityDbContextFactory.Create();
        _mockEventBus = new Mock<IEventBus>();
        _mockEventBus.Setup(e => e.PublishAsync(It.IsAny<ProfileCreatedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _handler = new CreateProfileRequestHandler(_mockContext.Object, _mockEventBus.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProfileSaveAndPublishEvent()
    {
        var request = new CreateProfileRequest
        {
            Profile = new ProfileDto()
        };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.Profile.Should().NotBeNull();
        _mockContext.Verify(c => c.Profiles.Add(It.IsAny<Profile>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockEventBus.Verify(e => e.PublishAsync(It.IsAny<ProfileCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPublishProfileCreatedEventWithCorrectProfileId()
    {
        var request = new CreateProfileRequest
        {
            Profile = new ProfileDto()
        };

        ProfileCreatedEvent? publishedEvent = null;
        _mockEventBus.Setup(e => e.PublishAsync(It.IsAny<ProfileCreatedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<ProfileCreatedEvent, CancellationToken>((evt, ct) => publishedEvent = evt)
            .Returns(Task.CompletedTask);

        var response = await _handler.Handle(request, CancellationToken.None);

        publishedEvent.Should().NotBeNull();
        publishedEvent!.ProfileId.Should().Be(response.Profile.ProfileId);
    }
}
