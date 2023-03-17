using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Commitments;

 public class GetDailyCommitmentsRequest : IRequest<GetDailyCommitmentsResponse> {
     public int ProfileId { get; set; }
 }

 public class GetDailyCommitmentsResponse
 {
     public IEnumerable<CommitmentDto> Commitments { get; set; }
 }

 public class GetDailyCommitmentsQueryHandler : IRequestHandler<GetDailyCommitmentsRequest, GetDailyCommitmentsResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetDailyCommitmentsQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDailyCommitmentsResponse> Handle(GetDailyCommitmentsRequest request, CancellationToken cancellationToken)
         => new GetDailyCommitmentsResponse()
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
