using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.ToDos
{
    public class RemoveToDoCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.ToDoId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public int ToDoId { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.ToDos.Remove(await _context.ToDos.FindAsync(request.ToDoId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
