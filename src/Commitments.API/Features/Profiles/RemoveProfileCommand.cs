using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Profiles
{
    public class RemoveProfileCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Profile.ProfileId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public Profile Profile { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Profiles.Remove(await _context.Profiles.FindAsync(request.Profile.ProfileId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
