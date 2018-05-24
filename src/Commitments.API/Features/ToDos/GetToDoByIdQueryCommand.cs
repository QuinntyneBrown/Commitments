using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.ToDos
{
    public class GetToDoByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.ToDoId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int ToDoId { get; set; }
        }

        public class Response
        {
            public ToDoApiModel ToDo { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    ToDo = ToDoApiModel.FromToDo(await _context.ToDos.FindAsync(request.ToDoId))
                };
        }
    }
}
