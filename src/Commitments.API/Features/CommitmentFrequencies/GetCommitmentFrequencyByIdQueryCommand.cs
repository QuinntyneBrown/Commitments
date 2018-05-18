using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.API.Features.CommitmentFrequencies
{
    public class GetCommitmentFrequencyByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CommitmentFrequencyId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int CommitmentFrequencyId { get; set; }
        }

        public class Response
        {
            public CommitmentFrequencyApiModel CommitmentFrequency { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    CommitmentFrequency = CommitmentFrequencyApiModel.FromCommitmentFrequency(await _context.CommitmentFrequencies.FindAsync(request.CommitmentFrequencyId))
                };
        }
    }
}
