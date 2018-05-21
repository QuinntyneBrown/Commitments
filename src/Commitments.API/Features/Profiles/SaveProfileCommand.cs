using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Profiles
{
    public class SaveProfileCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Profile.ProfileId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public ProfileApiModel Profile { get; set; }
        }

        public class Response
        {        	
            public int ProfileId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var profile = await _context.Profiles.FindAsync(request.Profile.ProfileId);

                if (profile == null) _context.Profiles.Add(profile = new Profile());

                profile.Name = request.Profile.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { ProfileId = profile.ProfileId };
            }
        }
    }
}
