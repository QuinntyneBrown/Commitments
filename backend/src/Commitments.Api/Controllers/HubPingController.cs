using Asp.Versioning;
using Commitments.Api.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Commitments.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/dev/hub-ping")]
[Authorize]
public sealed class HubPingController : ControllerBase
{
    private readonly IHubContext<CommitmentsHub> _hub;
    private readonly IHttpContextAccessor _http;
    private readonly IHostEnvironment _env;

    public HubPingController(IHubContext<CommitmentsHub> hub, IHttpContextAccessor http, IHostEnvironment env)
    {
        _hub = hub;
        _http = http;
        _env = env;
    }

    public sealed record PingRequest(string? Text);

    [HttpPost]
    public async Task<IActionResult> Ping([FromBody] PingRequest? body)
    {
        if (!_env.IsDevelopment()) return NotFound();

        var raw = _http.HttpContext!.Request.Headers["ProfileId"].FirstOrDefault();
        if (!Guid.TryParse(raw, out var profileId) || profileId == Guid.Empty)
            return BadRequest("Missing ProfileId");

        var group = $"profile:{profileId:D}".ToLowerInvariant();
        await _hub.Clients.Group(group).SendAsync("message",
            new { @event = "hubPing", text = body?.Text ?? "hello" });

        return Accepted();
    }
}
