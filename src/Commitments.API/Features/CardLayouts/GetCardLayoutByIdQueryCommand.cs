using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.CardLayouts
{
    public class GetCardLayoutByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CardLayoutId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int CardLayoutId { get; set; }
        }

        public class Response
        {
            public CardLayoutApiModel CardLayout { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    CardLayout = CardLayoutApiModel.FromCardLayout(await _context.CardLayouts.FindAsync(request.CardLayoutId))
                };
        }
    }
}
