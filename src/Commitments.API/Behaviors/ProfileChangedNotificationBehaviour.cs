using Commitments.API.Features.Profiles;
using Commitments.API.Hubs;
using Commitments.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Commitments.API.Behaviors
{
    public class ProfileChangedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>

    {
        private readonly IHubContext<AppHub> _hubContext;
        private readonly IAppDbContext _context;

        public ProfileChangedBehavior(IHubContext<AppHub> hubContext, IAppDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (typeof(TRequest) == typeof(SaveAvatarCommand.Request))
                return await (HandleSaveAvatarCommand(request as SaveAvatarCommand.Request, cancellationToken, response as SaveAvatarCommand.Response) as Task<TResponse>);

            return response;
        }

        public async Task<SaveAvatarCommand.Response> HandleSaveAvatarCommand(SaveAvatarCommand.Request request, CancellationToken cancellationToken, SaveAvatarCommand.Response response)
        {
            var profile = await _context.Profiles.FindAsync(response.ProfileId);

            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Profile] Changed",
                Payload = new { profile = ProfileApiModel.FromProfile(profile) }
            });

            return response;
        }

    }
}
