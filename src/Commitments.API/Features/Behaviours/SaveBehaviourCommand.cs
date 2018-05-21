using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Commitments.Core.Extensions;

namespace Commitments.API.Features.Behaviours
{
    public class SaveBehaviourCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Behaviour.BehaviourId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public BehaviourApiModel Behaviour { get; set; }
        }

        public class Response
        {            
            public int BehaviourId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var behaviour = await _context.Behaviours.FindAsync(request.Behaviour.BehaviourId);

                if (behaviour == null) _context.Behaviours.Add(behaviour = new Behaviour());

                behaviour.Name = request.Behaviour.Name;
                behaviour.Slug = request.Behaviour.Name.GenerateSlug();
                behaviour.Description = request.Behaviour.Description;
                behaviour.BehaviourTypeId = request.Behaviour.BehaviourTypeId;
                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { BehaviourId = behaviour.BehaviourId };
            }
        }
    }
}
