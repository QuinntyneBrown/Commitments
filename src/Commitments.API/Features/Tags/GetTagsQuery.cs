using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.Tags
{
    public class GetTagsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<TagApiModel> Tags { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Tags = await _context.Tags.Select(x => TagApiModel.FromTag(x)).ToListAsync()
                };
        }
    }
}
