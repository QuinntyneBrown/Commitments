using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.Api.Features.Profiles
{
    public class GetProfileByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.ProfileId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int ProfileId { get; set; }
        }

        public class Response
        {
            public ProfileApiModel Profile { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Profile = ProfileApiModel.FromProfile(await _context.Profiles.FindAsync(request.ProfileId))
                };
        }
    }
}
