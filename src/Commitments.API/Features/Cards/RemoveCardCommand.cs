using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Cards
{
    public class RemoveCardCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CardId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public int CardId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Cards.Remove(await _context.Cards.FindAsync(request.CardId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
