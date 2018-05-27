using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.Profiles
{
    public class SaveAvatarCommand
    {
        public class Request : IRequest<Response> {

            public int ProfileId { get; set; }
            public string AvatarUrl { get; set; }
        }

        public class Response
        {
            public int ProfileId { get;set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {
                var profile = _context.Profiles.Find(request.ProfileId);
                profile.AvatarUrl = request.AvatarUrl;
                await _context.SaveChangesAsync(cancellationToken);
                return new Response() {
                    ProfileId = profile.ProfileId
                };
            }
        }
    }
}
