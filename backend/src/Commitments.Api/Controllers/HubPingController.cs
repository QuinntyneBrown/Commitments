using Asp.Versioning;
using Commitments.Shared;
using Commitments.Shared.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Commitments.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/dev/hub-ping")]
[Authorize]
public sealed class HubPingController : ControllerBase
{
    private readonly IRealtimePublisher _publisher;
    private readonly IHttpContextAccessor _http;
    private readonly IHostEnvironment _env;

    public HubPingController(IRealtimePublisher publisher, IHttpContextAccessor http, IHostEnvironment env)
    {
        _publisher = publisher;
        _http = http;
        _env = env;
    }

    public sealed record PingRequest(string? Text);

    public sealed record PingPayload(string Text);

    [HttpPost]
    public async Task<IActionResult> Ping([FromBody] PingRequest? body)
    {
        if (!_env.IsDevelopment()) return NotFound();

        if (!_http.HttpContext!.TryGetProfileId(out var profileId))
            return BadRequest("Missing ProfileId");

        await _publisher.PublishToProfileAsync(
            profileId,
            @event: "hubPing",
            payload: new PingPayload(body?.Text ?? "hello"));

        return Accepted();
    }
}
