using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.Profiles
{
    public class GetProfilesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<ProfileApiModel> Profiles { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Profiles = await _context.Profiles.Select(x => ProfileApiModel.FromProfile(x)).ToListAsync()
                };
        }
    }
}
