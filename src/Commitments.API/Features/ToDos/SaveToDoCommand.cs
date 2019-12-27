using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.Api.Features.ToDos
{
    public class SaveToDoCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.ToDo.ToDoId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public ToDoApiModel ToDo { get; set; }
        }

        public class Response
        {			
            public int ToDoId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var toDo = await _context.ToDos.FindAsync(request.ToDo.ToDoId);

                if (toDo == null) _context.ToDos.Add(toDo = new ToDo());

                toDo.Name = request.ToDo.Name;
                toDo.CompletedOn = request.ToDo.CompletedOn;
                toDo.DueOn = request.ToDo.DueOn;
                toDo.Description = request.ToDo.Description;
                toDo.ProfileId = request.ToDo.ProfileId;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { ToDoId = toDo.ToDoId };
            }
        }
    }
}
