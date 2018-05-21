using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Dashboards
{
    public class RemoveDashboardCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Dashboard.DashboardId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public Dashboard Dashboard { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Dashboards.Remove(await _context.Dashboards.FindAsync(request.Dashboard.DashboardId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
