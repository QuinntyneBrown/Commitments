using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.BehaviourTypes
{
    public class GetBehaviourTypesQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<BehaviourTypeApiModel> BehaviourTypes { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    BehaviourTypes = await _context.BehaviourTypes.Select(x => BehaviourTypeApiModel.FromBehaviourType(x)).ToListAsync()
                };
        }
    }
}
