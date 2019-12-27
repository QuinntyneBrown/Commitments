using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.Commitments
{
    public class GetCommitmentsQuery
    {
        public class Request : IRequest<Response> { }

        public class Response
        {
            public IEnumerable<CommitmentApiModel> Commitments { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Commitments = await _context.Commitments
                    .Include(x => x.Behaviour)
                    .Include("Behaviour.BehaviourType")
                    .Include(x => x.CommitmentFrequencies)
                    .Include("CommitmentFrequencies.Frequency")
                    .Include("CommitmentFrequencies.Frequency.FrequencyType")
                    .Select(x => CommitmentApiModel.FromCommitment(x)).ToListAsync()
                };
        }
    }
}
