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
                RuleFor(request => request.ActivityId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public int ActivityId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Activities.Remove(await _context.Activities.FindAsync(request.ActivityId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
