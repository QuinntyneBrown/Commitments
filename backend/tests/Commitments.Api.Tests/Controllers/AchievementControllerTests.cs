using Commitments.Controllers;
using Commitments.Features.Achievement;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class AchievementControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly AchievementController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public AchievementControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = _profileId.ToString();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _controller = new AchievementController(_mockHttpContextAccessor.Object, _mockSender.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnAchievementsResponse()
    {
        // Arrange
        var expectedResponse = new GetAchievementsResponse
        {
            Achievements = new List<AchievementDto>
            {
                new AchievementDto { AchievementId = Guid.NewGuid() },
                new AchievementDto { AchievementId = Guid.NewGuid() }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetAchievementsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Achievements.Should().HaveCount(2);
    }

    [Fact]
    public async Task Get_ShouldSendRequestWithProfileIdFromHttpContext()
    {
        // Arrange
        var expectedResponse = new GetAchievementsResponse
        {
            Achievements = new List<AchievementDto>()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetAchievementsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _controller.Get();

        // Assert
        _mockSender.Verify(s => s.Send(
            It.Is<GetAchievementsRequest>(r => r.ProfileId == _profileId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Get_NoAchievements_ShouldReturnEmptyList()
    {
        // Arrange
        var expectedResponse = new GetAchievementsResponse
        {
            Achievements = new List<AchievementDto>()
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetAchievementsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Achievements.Should().BeEmpty();
    }
}
