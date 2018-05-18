using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.BehaviourTypes
{
    public class GetBehaviourTypeByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.BehaviourTypeId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int BehaviourTypeId { get; set; }
        }

        public class Response
        {
            public BehaviourTypeApiModel BehaviourType { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    BehaviourType = BehaviourTypeApiModel.FromBehaviourType(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId))
                };
        }
    }
}
