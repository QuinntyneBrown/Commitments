using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;

namespace Commitments.Api.Features.DashboardCards
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
            public IMediator _mediator { get; set; }
            public Handler(IAppDbContext context, IMediator mediator) {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dashboardCardIds = new List<int>();

                foreach(var dashboardCard in request.DashboardCards)
                {                    
                    var response = await _mediator.Send(new SaveDashboardCardCommand.Request() { DashboardCard = dashboardCard });
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
