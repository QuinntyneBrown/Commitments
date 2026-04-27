using Commitments.Api.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Commitments.Api.Tests.Middleware;

public class JwtQueryStringAuthMiddlewareTests
{
    private static async Task<HttpContext> Invoke(string path, string queryString, Action<HttpContext>? mutate = null)
    {
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.QueryString = new QueryString(queryString);
        mutate?.Invoke(context);

        var middleware = new JwtQueryStringAuthMiddleware(_ => Task.CompletedTask);
        await middleware.InvokeAsync(context);
        return context;
    }

    [Fact]
    public async Task HubPath_WithTokenQuery_SetsAuthorizationHeader()
    {
        var ctx = await Invoke("/hub", "?token=abc123");

        ctx.Request.Headers["Authorization"].ToString().Should().Be("Bearer abc123");
    }

    [Fact]
    public async Task HubPath_WithProfileIdQuery_SetsProfileIdHeader()
    {
        var profileId = Guid.NewGuid().ToString();
        var ctx = await Invoke("/hub", $"?profileId={profileId}");

        ctx.Request.Headers["ProfileId"].ToString().Should().Be(profileId);
    }

    [Fact]
    public async Task HubPath_WithBothQueryValues_SetsBothHeaders()
    {
        var profileId = Guid.NewGuid().ToString();
        var ctx = await Invoke("/hub", $"?token=abc&profileId={profileId}");

        ctx.Request.Headers["Authorization"].ToString().Should().Be("Bearer abc");
        ctx.Request.Headers["ProfileId"].ToString().Should().Be(profileId);
    }

    [Fact]
    public async Task NonHubPath_WithTokenQuery_DoesNotSetAuthorizationHeader()
    {
        var ctx = await Invoke("/api/v1.0/things", "?token=abc");

        ctx.Request.Headers.ContainsKey("Authorization").Should().BeFalse();
    }

    [Fact]
    public async Task HubPath_WithExistingAuthorizationHeader_DoesNotOverwrite()
    {
        var ctx = await Invoke("/hub", "?token=abc",
            c => c.Request.Headers["Authorization"] = "Bearer existing");

        ctx.Request.Headers["Authorization"].ToString().Should().Be("Bearer existing");
    }

    [Fact]
    public async Task HubPath_WithoutQueryValues_LeavesHeadersUntouched()
    {
        var ctx = await Invoke("/hub", "");

        ctx.Request.Headers.ContainsKey("Authorization").Should().BeFalse();
        ctx.Request.Headers.ContainsKey("ProfileId").Should().BeFalse();
    }
}
