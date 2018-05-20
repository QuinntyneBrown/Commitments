using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.DashboardCards
{
    public class GetDashboardCardByIdsQuery
    {
        public class Request : IRequest<Response> {
            public int[] DashboardCardIds { get; set; }
        }

        public class Response
        {
            public IEnumerable<DashboardCardApiModel> DashboardCards { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    DashboardCards = await _context.DashboardCards
                    .Where(x => request.DashboardCardIds.Contains(x.DashboardId))
                    .Select(x => DashboardCardApiModel.FromDashboardCard(x)).ToListAsync()
                };
        }
    }
}
