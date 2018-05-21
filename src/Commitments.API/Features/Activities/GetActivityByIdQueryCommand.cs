using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.Activities
{
    public class GetActivityByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.ActivityId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int ActivityId { get; set; }
        }

        public class Response
        {
            public ActivityApiModel Activity { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Activity = ActivityApiModel.FromActivity(await _context.Activities.FindAsync(request.ActivityId))
                };
        }
    }
}
