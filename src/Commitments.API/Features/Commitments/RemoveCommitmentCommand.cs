using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Commitments
{
    public class RemoveCommitmentCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Commitment.CommitmentId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public Commitment Commitment { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Commitments.Remove(await _context.Commitments.FindAsync(request.Commitment.CommitmentId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
