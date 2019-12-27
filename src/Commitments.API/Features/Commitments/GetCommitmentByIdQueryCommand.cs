using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.Api.Features.Commitments
{
    public class GetCommitmentByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.CommitmentId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int CommitmentId { get; set; }
        }

        public class Response
        {
            public CommitmentApiModel Commitment { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Commitment = CommitmentApiModel.FromCommitment(await _context.Commitments.FindAsync(request.CommitmentId))
                };
        }
    }
}
