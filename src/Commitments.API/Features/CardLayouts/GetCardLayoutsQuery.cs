using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.CardLayouts
{
    public class GetCardLayoutsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CardLayoutApiModel> CardLayouts { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    CardLayouts = await _context.CardLayouts.Select(x => CardLayoutApiModel.FromCardLayout(x)).ToListAsync()
                };
        }
    }
}
