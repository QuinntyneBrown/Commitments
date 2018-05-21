using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Activities
{
    public class RemoveActivityCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Activity.ActivityId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Activities.Remove(await _context.Activities.FindAsync(request.Activity.ActivityId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
