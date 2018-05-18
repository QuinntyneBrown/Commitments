using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.CommitmentFrequencies
{
    public class RemoveCommitmentFrequencyCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CommitmentFrequency.CommitmentFrequencyId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public CommitmentFrequency CommitmentFrequency { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.CommitmentFrequencies.Remove(await _context.CommitmentFrequencies.FindAsync(request.CommitmentFrequency.CommitmentFrequencyId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
