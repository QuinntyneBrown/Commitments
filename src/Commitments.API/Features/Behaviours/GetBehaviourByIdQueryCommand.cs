using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.Behaviours
{
    public class GetBehaviourByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.BehaviourId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int BehaviourId { get; set; }
        }

        public class Response
        {
            public BehaviourApiModel Behaviour { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Behaviour = BehaviourApiModel.FromBehaviour(await _context.Behaviours.FindAsync(request.BehaviourId))
                };
        }
    }
}
