using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Commitments.Api.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AppHub: Hub
    {        
        public async Task Send(string message) {                        
            await Clients.All.SendAsync("message", message);
        }
    }
}
