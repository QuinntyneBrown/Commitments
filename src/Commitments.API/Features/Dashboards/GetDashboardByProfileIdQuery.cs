using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.Dashboards
{
    public class GetDashboardByProfileIdQuery
    {
        public class Request : IRequest<Response> {
            public int ProfileId { get; set; }
        }

        public class Response
        {
            public DashboardApiModel Dashboard { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Dashboard = DashboardApiModel.FromDashboard(await _context.Dashboards
                        .Include(x => x.DashboardCards)
                        .SingleAsync(x => x.ProfileId == request.ProfileId))
                };
        }
    }
}
