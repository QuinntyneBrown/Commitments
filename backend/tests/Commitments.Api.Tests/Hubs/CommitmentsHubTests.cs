using Commitments.Api.Hubs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Commitments.Api.Tests.Hubs;

public class CommitmentsHubTests
{
    private readonly Mock<IHttpContextAccessor> _httpAccessor = new();
    private readonly Mock<ILogger<CommitmentsHub>> _logger = new();
    private readonly Mock<IGroupManager> _groups = new();
    private readonly Mock<HubCallerContext> _context = new();
    private readonly Guid _profileId = Guid.Parse("0e3b1a2c-1234-4abc-9def-001122334455");

    private CommitmentsHub CreateHub(string? profileIdHeader)
    {
        var httpContext = new DefaultHttpContext();
        if (profileIdHeader is not null)
            httpContext.Request.Headers["ProfileId"] = profileIdHeader;
        _httpAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _context.Setup(c => c.ConnectionId).Returns("conn-1");

        return new CommitmentsHub(_httpAccessor.Object, _logger.Object)
        {
            Groups = _groups.Object,
            Context = _context.Object
        };
    }

    [Fact]
    public async Task OnConnectedAsync_WithValidProfileId_JoinsLowercaseProfileGroup()
    {
        var hub = CreateHub(_profileId.ToString().ToUpperInvariant());

        await hub.OnConnectedAsync();

        var expectedGroup = $"profile:{_profileId:D}".ToLowerInvariant();
        _groups.Verify(g => g.AddToGroupAsync("conn-1", expectedGroup, It.IsAny<CancellationToken>()), Times.Once);
        _context.Verify(c => c.Abort(), Times.Never);
    }

    [Fact]
    public async Task OnConnectedAsync_WithMissingProfileId_AbortsConnection()
    {
        var hub = CreateHub(profileIdHeader: null);

        await hub.OnConnectedAsync();

        _context.Verify(c => c.Abort(), Times.Once);
        _groups.Verify(g => g.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task OnConnectedAsync_WithEmptyGuid_AbortsConnection()
    {
        var hub = CreateHub(Guid.Empty.ToString());

        await hub.OnConnectedAsync();

        _context.Verify(c => c.Abort(), Times.Once);
        _groups.Verify(g => g.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
