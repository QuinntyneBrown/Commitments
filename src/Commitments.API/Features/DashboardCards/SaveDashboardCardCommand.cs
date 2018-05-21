using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Newtonsoft.Json;

namespace Commitments.API.Features.DashboardCards
{
    public class SaveDashboardCardCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.DashboardCard.DashboardCardId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public DashboardCardApiModel DashboardCard { get; set; }
        }

        public class Response
        {        	
            public int DashboardCardId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dashboardCard = await _context.DashboardCards.FindAsync(request.DashboardCard.DashboardCardId);

                if (dashboardCard == null) _context.DashboardCards.Add(dashboardCard = new DashboardCard());

                dashboardCard.DashboardCardId = request.DashboardCard.DashboardCardId;
                dashboardCard.DashboardId = request.DashboardCard.DashboardId;
                dashboardCard.CardId = request.DashboardCard.CardId;
                dashboardCard.Options = JsonConvert.SerializeObject(request.DashboardCard.Options);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { DashboardCardId = dashboardCard.DashboardCardId };
            }
        }
    }
}
