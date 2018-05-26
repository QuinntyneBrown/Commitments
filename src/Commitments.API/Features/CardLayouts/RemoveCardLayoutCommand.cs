using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.CardLayouts
{
    public class RemoveCardLayoutCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CardLayoutId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public int CardLayoutId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.CardLayouts.Remove(await _context.CardLayouts.FindAsync(request.CardLayoutId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
