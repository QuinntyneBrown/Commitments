using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Commitments.Core.Interfaces;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;

namespace Commitments.Api.Features.Tags
{
    public class SaveTagCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Tag.TagId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public TagApiModel Tag { get; set; }
        }

        public class Response
        {            
            public int TagId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IAppDbContext _context;

            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var tag = await _context.Tags.FindAsync(request.Tag.TagId);

                if (tag == null) _context.Tags.Add(tag = new Tag());

                tag.Name = request.Tag.Name;

                tag.Slug = request.Tag.Name.GenerateSlug();

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { TagId = tag.TagId };
            }
        }
    }
}
