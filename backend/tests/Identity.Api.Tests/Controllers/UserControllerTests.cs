using Identity.Controllers;
using Identity.Features.User;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace Identity.Api.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<UserController>> _mockLogger;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<UserController>>();
        _controller = new UserController(_mockSender.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnUsers()
    {
        var expectedResponse = new GetUsersResponse { Users = new List<UserDto>() };
        _mockSender.Setup(s => s.Send(It.IsAny<GetUsersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Get(CancellationToken.None);

        result.Value.Should().NotBeNull();
    }
}
