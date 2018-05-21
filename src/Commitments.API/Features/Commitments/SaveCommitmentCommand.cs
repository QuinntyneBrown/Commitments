using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.Commitments
{
    public class SaveCommitmentCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Commitment.CommitmentId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public CommitmentApiModel Commitment { get; set; }
        }

        public class Response
        {            
            public int CommitmentId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var commitment = await _context.Commitments
                    .Include(x => x.CommitmentFrequencies)
                    .Include("CommitmentFrequencies.Frequency")
                    .SingleOrDefaultAsync(x => x.CommitmentId == request.Commitment.CommitmentId);

                if (commitment == null) _context.Commitments.Add(commitment = new Commitment());

                commitment.BehaviourId = request.Commitment.BehaviourId;
                commitment.ProfileId = request.Commitment.ProfileId;

                commitment.CommitmentFrequencies.Clear();

                foreach (var cf in request.Commitment.CommitmentFrequencies) {                    
                    commitment.CommitmentFrequencies.Add(new CommitmentFrequency()
                    {
                        FrequencyId = cf.FrequencyId
                    });
                }
                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { CommitmentId = commitment.CommitmentId };
            }
        }
    }
}
