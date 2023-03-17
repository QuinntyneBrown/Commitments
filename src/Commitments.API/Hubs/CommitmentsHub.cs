using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace Commitments.Api.Hubs;

[Authorize(AuthenticationSchemes = "Bearer")]
public class CommitmentsHub: Hub<ICommitmentsClient>
{        
    
}
