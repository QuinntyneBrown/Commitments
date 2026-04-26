using Identity.Controllers;
using Identity.Features.Profile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace Identity.Api.Tests.Controllers;

public class ProfileControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<ProfileController>> _mockLogger;
    private readonly ProfileController _controller;

    public ProfileControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<ProfileController>>();
        _controller = new ProfileController(_mockSender.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreateProfileResponse()
    {
        var request = new CreateProfileRequest { Profile = new ProfileDto { ProfileId = Guid.NewGuid() } };
        var expectedResponse = new CreateProfileResponse { Profile = request.Profile };
        _mockSender.Setup(s => s.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Create(request, CancellationToken.None);

        result.Value.Should().NotBeNull();
        result.Value!.Profile.ProfileId.Should().Be(request.Profile.ProfileId);
    }

    [Fact]
    public async Task Get_ShouldReturnProfiles()
    {
        var expectedResponse = new GetProfilesResponse { Profiles = new List<ProfileDto>() };
        _mockSender.Setup(s => s.Send(It.IsAny<GetProfilesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Get(CancellationToken.None);

        result.Value.Should().NotBeNull();
        result.Value!.Profiles.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_WhenProfileExists_ShouldReturnProfile()
    {
        var profileId = Guid.NewGuid();
        var expectedResponse = new GetProfileByIdResponse { Profile = new ProfileDto { ProfileId = profileId } };
        _mockSender.Setup(s => s.Send(It.IsAny<GetProfileByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.GetById(profileId, CancellationToken.None);

        result.Value.Should().NotBeNull();
        result.Value!.Profile!.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public async Task GetById_WhenProfileNotFound_ShouldReturnNotFound()
    {
        var profileId = Guid.NewGuid();
        var expectedResponse = new GetProfileByIdResponse { Profile = null };
        _mockSender.Setup(s => s.Send(It.IsAny<GetProfileByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.GetById(profileId, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteProfileResponse()
    {
        var profileId = Guid.NewGuid();
        var expectedResponse = new DeleteProfileResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteProfileRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Delete(profileId, CancellationToken.None);

        result.Value.Should().NotBeNull();
    }
}
