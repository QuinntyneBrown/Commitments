using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.Api.Features.Tags
{
    public class GetTagByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.TagId).NotEqual(default(int));
            }
        }

        public class Request : IRequest<Response> {
            public int TagId { get; set; }
        }

        public class Response
        {
            public TagApiModel Tag { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Tag = TagApiModel.FromTag(await _context.Tags.FindAsync(request.TagId))
                };
        }
    }
}
