using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.Activities
{
    public class SaveActivityCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Activity.ActivityId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public ActivityApiModel Activity { get; set; }
        }

        public class Response
        {			
            public int ActivityId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.ActivityId);

                if (activity == null) _context.Activities.Add(activity = new Activity());

                activity.BehaviourId = request.Activity.BehaviourId;
                activity.ProfileId = request.Activity.ProfileId;
                activity.PerformedOn = request.Activity.PerformedOn;
                activity.Description = request.Activity.Description;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { ActivityId = activity.ActivityId };
            }
        }
    }
}
