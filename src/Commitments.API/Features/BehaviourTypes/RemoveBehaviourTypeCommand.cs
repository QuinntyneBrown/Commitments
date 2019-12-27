using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.Api.Features.BehaviourTypes
{
    public class RemoveBehaviourTypeCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.BehaviourTypeId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public int BehaviourTypeId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.BehaviourTypes.Remove(await _context.BehaviourTypes.FindAsync(request.BehaviourTypeId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
