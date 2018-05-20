using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.DashboardCards
{
    public class SaveDashboardCardRangeCommand
    {
        public class Request : IRequest<Response> {
            public IEnumerable<DashboardCardApiModel> DashboardCards { get; set; }
        }

        public class Response
        {
            public IEnumerable<int> DashboardCardIds { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dashboardCardIds = new List<int>();

                foreach(var dashboardCard in request.DashboardCards)
                {
                    var handler = new SaveDashboardCardCommand.Handler(_context);
                    var response = await handler.Handle(new SaveDashboardCardCommand.Request() { DashboardCard = dashboardCard }, cancellationToken);
                    dashboardCardIds.Add(response.DashboardCardId);
                }

                return new Response()
                {
                    DashboardCardIds = dashboardCardIds
                };
            }

        }
    }
}
