using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Commitments;

 public class GetDailyCommitmentsQueryRequest : IRequest<GetDailyCommitmentsQueryResponse> {
     public int ProfileId { get; set; }
 }

 public class GetDailyCommitmentsQueryResponse
 {
     public IEnumerable<CommitmentDto> Commitments { get; set; }
 }

 public class GetDailyCommitmentsQueryHandler : IRequestHandler<GetDailyCommitmentsQueryRequest, GetDailyCommitmentsQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetDailyCommitmentsQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDailyCommitmentsQueryResponse> Handle(GetDailyCommitmentsQueryRequest request, CancellationToken cancellationToken)
         => new GetDailyCommitmentsQueryResponse()
         {
             Commitments = await _context.Commitments
             .Include(x => x.Behaviour)
             .Include("Behaviour.BehaviourType")
             .Include(x => x.CommitmentFrequencies)
             .Include("CommitmentFrequencies.Frequency")
             .Include("CommitmentFrequencies.Frequency.FrequencyType")
             .Where(x => x.ProfileId == request.ProfileId && x.CommitmentFrequencies.Any(f => f.Frequency.FrequencyType.Name == "per day" ))
             .Select(x => CommitmentDto.FromCommitment(x)).ToListAsync()
         };
 }
