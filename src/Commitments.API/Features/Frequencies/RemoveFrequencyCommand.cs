using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Frequencies
{
    public class RemoveFrequencyCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Frequency.FrequencyId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public Frequency Frequency { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Frequencies.Remove(await _context.Frequencies.FindAsync(request.Frequency.FrequencyId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
