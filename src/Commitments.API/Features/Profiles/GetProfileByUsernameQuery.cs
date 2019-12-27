using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.Profiles
{
    public class GetProfileByUsernameQuery
    {
        public class Request : IRequest<Response> {
            public string Username { get; set; }
        }

        public class Response
        {
            public ProfileApiModel Profile { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) 
                => new Response()
                {
                    Profile = ProfileApiModel.FromProfile(await _context.Profiles.SingleAsync(x => x.User.Username == request.Username))
                };
        }
    }
}
