using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.Commitments
{
    public class GetDailyCommitmentsQuery
    {
        public class Request : IRequest<Response> {
            public int ProfileId { get; set; }
        }

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
                    .Include(x => x.CommitmentFrequencies)
                    .Include("CommitmentFrequencies.Frequency")
                    .Include("CommitmentFrequencies.Frequency.FrequencyType")
                    .Where(x => x.ProfileId == request.ProfileId && x.CommitmentFrequencies.Any(f => f.Frequency.FrequencyType.Name == "per day" ))
                    .Select(x => CommitmentApiModel.FromCommitment(x)).ToListAsync()
                };
        }
    }
}
