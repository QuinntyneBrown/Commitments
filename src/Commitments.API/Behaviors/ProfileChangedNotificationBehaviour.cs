using Commitments.Api.Features.Profiles;
using Commitments.Api.Hubs;
using Commitments.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Api.Behaviors;

public class ProfileChangedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

{
    private readonly IHubContext<CommitmentsHub> _hubContext;
    private readonly ICommimentsDbContext _context;

    public ProfileChangedBehavior(IHubContext<CommitmentsHub> hubContext, ICommimentsDbContext context)
    {
        _hubContext = hubContext;
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var response = await next();

        if (typeof(TRequest) == typeof(SaveAvatarCommandRequest))
            return await (HandleSaveAvatarCommand(request as SaveAvatarCommandRequest, cancellationToken, response as SaveAvatarCommandResponse) as Task<TResponse>);

        return response;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public async Task<SaveAvatarCommandResponse> HandleSaveAvatarCommand(SaveAvatarCommandRequest request, CancellationToken cancellationToken, SaveAvatarCommandResponse response)
    {
        var profile = await _context.Profiles.FindAsync(response.ProfileId);

        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Profile] Changed",
            Payload = new { profile = ProfileDto.FromProfile(profile) }
        });

        return response;
    }

}
