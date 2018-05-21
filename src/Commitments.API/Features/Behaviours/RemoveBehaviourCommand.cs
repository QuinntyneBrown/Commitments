using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Behaviours
{
    public class RemoveBehaviourCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.BehaviourId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public int BehaviourId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Behaviours.Remove(await _context.Behaviours.FindAsync(request.BehaviourId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
