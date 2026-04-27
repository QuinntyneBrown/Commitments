using Commitments.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Commitments.Api.Hubs;

[Authorize]
public sealed class CommitmentsHub : Hub
{
    private readonly IHttpContextAccessor _http;
    private readonly ILogger<CommitmentsHub> _log;

    public CommitmentsHub(IHttpContextAccessor http, ILogger<CommitmentsHub> log)
    {
        _http = http;
        _log = log;
    }

    public override async Task OnConnectedAsync()
    {
        if (_http.HttpContext is null || !_http.HttpContext.TryGetProfileId(out var profileId))
        {
            _log.LogWarning("Hub connect rejected: missing/invalid ProfileId for {ConnectionId}", Context.ConnectionId);
            Context.Abort();
            return;
        }

        var group = $"profile:{profileId:D}".ToLowerInvariant();
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        _log.LogInformation("Hub connect: {ConnectionId} joined {Group}", Context.ConnectionId, group);

        await base.OnConnectedAsync();
    }
}
