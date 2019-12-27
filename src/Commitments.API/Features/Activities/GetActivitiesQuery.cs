using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.Activities
{
    public class GetActivitiesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<ActivityApiModel> Activities { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Activities = await _context.Activities
                    .Include(x => x.Behaviour)
                    .Include("Behaviour.BehaviourType")
                    .Select(x => ActivityApiModel.FromActivity(x)).ToListAsync()
                };
        }
    }
}
