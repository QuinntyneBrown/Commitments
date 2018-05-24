using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.ToDos
{
    public class GetToDosQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<ToDoApiModel> ToDos { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    ToDos = await _context.ToDos.Select(x => ToDoApiModel.FromToDo(x)).ToListAsync()
                };
        }
    }
}
