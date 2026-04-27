using Commitments.Api.Controllers;
using Commitments.Shared.Realtime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class HubPingControllerTests
{
    private readonly Mock<IRealtimePublisher> _publisher = new();
    private readonly Mock<IHttpContextAccessor> _httpAccessor = new();
    private readonly Guid _profileId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

    private HubPingController CreateController(Guid? profileId, string environmentName = "Development")
    {
        var httpContext = new DefaultHttpContext();
        if (profileId is not null)
            httpContext.Request.Headers["ProfileId"] = profileId.ToString();
        _httpAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        var env = new StubHostEnvironment(environmentName);
        return new HubPingController(_publisher.Object, _httpAccessor.Object, env);
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
    public async Task Ping_WithValidProfileId_PublishesHubPingEventToProfile()
    {
        var controller = CreateController(_profileId);

        await controller.Ping(new HubPingController.PingRequest("hello"));

        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "hubPing",
            It.Is<HubPingController.PingPayload>(x => x.Text == "hello"),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Ping_WithNullBody_UsesHelloDefault()
    {
        var controller = CreateController(_profileId);

        var result = await controller.Ping(body: null);

        result.Should().BeOfType<AcceptedResult>();
        _publisher.Verify(p => p.PublishToProfileAsync(
            _profileId,
            "hubPing",
            It.Is<HubPingController.PingPayload>(x => x.Text == "hello"),
            It.IsAny<Guid?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Ping_WithMissingProfileId_ReturnsBadRequest()
    {
        var controller = CreateController(profileId: null);

        var result = await controller.Ping(new HubPingController.PingRequest("hello"));

        result.Should().BeOfType<BadRequestObjectResult>();
        _publisher.Verify(p => p.PublishToProfileAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<HubPingController.PingPayload>(),
            It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Ping_OutsideDevelopment_ReturnsNotFound()
    {
        var controller = CreateController(_profileId, environmentName: "Production");

        var result = await controller.Ping(new HubPingController.PingRequest("hello"));

        result.Should().BeOfType<NotFoundResult>();
        _publisher.Verify(p => p.PublishToProfileAsync(
            It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<HubPingController.PingPayload>(),
            It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
