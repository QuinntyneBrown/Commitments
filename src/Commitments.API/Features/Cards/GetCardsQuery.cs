using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.Cards
{
    public class GetCardsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CardApiModel> Cards { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Cards = await _context.Cards.Select(x => CardApiModel.FromCard(x)).ToListAsync()
                };
        }
    }
}
