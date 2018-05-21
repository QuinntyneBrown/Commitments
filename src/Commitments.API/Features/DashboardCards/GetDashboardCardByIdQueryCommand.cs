using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.DashboardCards
{
    public class GetDashboardCardByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.DashboardCardId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int DashboardCardId { get; set; }
        }

        public class Response
        {
            public DashboardCardApiModel DashboardCard { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    DashboardCard = DashboardCardApiModel.FromDashboardCard(await _context.DashboardCards.FindAsync(request.DashboardCardId))
                };
        }
    }
}
