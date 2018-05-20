using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.DashboardCards
{
    public class RemoveDashboardCardCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.DashboardCard.DashboardCardId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public DashboardCard DashboardCard { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.DashboardCards.Remove(await _context.DashboardCards.FindAsync(request.DashboardCard.DashboardCardId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
