using Commitments.Api.Controllers;
using Commitments.Api.Hubs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class HubPingControllerTests
{
    private readonly Mock<IHubContext<CommitmentsHub>> _hub = new();
    private readonly Mock<IHubClients> _clients = new();
    private readonly Mock<IClientProxy> _clientProxy = new();
    private readonly Mock<IHttpContextAccessor> _httpAccessor = new();
    private readonly Guid _profileId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    public HubPingControllerTests()
    {
        _clients.Setup(c => c.Group(It.IsAny<string>())).Returns(_clientProxy.Object);
        _hub.Setup(h => h.Clients).Returns(_clients.Object);
    }

    private HubPingController CreateController(Guid? profileId, string environmentName = "Development")
    {
        var httpContext = new DefaultHttpContext();
        if (profileId is not null)
            httpContext.Request.Headers["ProfileId"] = profileId.ToString();
        _httpAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        var env = new StubHostEnvironment(environmentName);
        return new HubPingController(_hub.Object, _httpAccessor.Object, env);
    }

    private sealed class StubHostEnvironment : IHostEnvironment
    {
        public StubHostEnvironment(string environmentName) => EnvironmentName = environmentName;
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; } = "Tests";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }

    [Fact]
    public async Task Ping_WithValidProfileId_ReturnsAccepted()
    {
        var controller = CreateController(_profileId);

        var result = await controller.Ping(new HubPingController.PingRequest("hello"));

        result.Should().BeOfType<AcceptedResult>();
    }

    [Fact]
    public async Task Ping_WithValidProfileId_SendsMessageToProfileGroup()
    {
        var controller = CreateController(_profileId);

        await controller.Ping(new HubPingController.PingRequest("hello"));

        var expectedGroup = $"profile:{_profileId:D}".ToLowerInvariant();
        _clients.Verify(c => c.Group(expectedGroup), Times.Once);
        _clientProxy.Verify(p => p.SendCoreAsync("message", It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Ping_WithNullBody_UsesHelloDefault()
    {
        var controller = CreateController(_profileId);

        var result = await controller.Ping(body: null);

        result.Should().BeOfType<AcceptedResult>();
        _clientProxy.Verify(p => p.SendCoreAsync("message", It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Ping_WithMissingProfileId_ReturnsBadRequest()
    {
        var controller = CreateController(profileId: null);

        var result = await controller.Ping(new HubPingController.PingRequest("hello"));

        result.Should().BeOfType<BadRequestObjectResult>();
        _clientProxy.Verify(p => p.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Ping_OutsideDevelopment_ReturnsNotFound()
    {
        var controller = CreateController(_profileId, environmentName: "Production");

        var result = await controller.Ping(new HubPingController.PingRequest("hello"));

        result.Should().BeOfType<NotFoundResult>();
        _clientProxy.Verify(p => p.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
