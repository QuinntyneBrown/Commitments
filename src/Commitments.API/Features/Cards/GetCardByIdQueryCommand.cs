using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.Cards
{
    public class GetCardByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CardId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int CardId { get; set; }
        }

        public class Response
        {
            public CardApiModel Card { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Card = CardApiModel.FromCard(await _context.Cards.FindAsync(request.CardId))
                };
        }
    }
}
