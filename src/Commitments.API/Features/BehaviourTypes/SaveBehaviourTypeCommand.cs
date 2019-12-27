using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Commitments.Api.Features.BehaviourTypes
{
    public class SaveBehaviourTypeCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.BehaviourType.BehaviourTypeId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public BehaviourTypeApiModel BehaviourType { get; set; }
        }

        public class Response
        {            
            public int BehaviourTypeId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var behaviourType = await _context.BehaviourTypes.FindAsync(request.BehaviourType.BehaviourTypeId);

                if (behaviourType == null) _context.BehaviourTypes.Add(behaviourType = new BehaviourType());

                behaviourType.Name = request.BehaviourType.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { BehaviourTypeId = behaviourType.BehaviourTypeId };
            }
        }
    }
}
