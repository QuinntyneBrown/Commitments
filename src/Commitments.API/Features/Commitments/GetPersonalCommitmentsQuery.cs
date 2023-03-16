using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Commitments;

 public class GetPersonalCommitmentsQueryRequest : IRequest<GetPersonalCommitmentsQueryResponse> {
     public int ProfileId { get; set; }
 }

 public class GetPersonalCommitmentsQueryResponse
 {
     public IEnumerable<CommitmentApiModel> Commitments { get; set; }
 }

 public class GetPersonalCommitmentsQueryHandler : IRequestHandler<GetPersonalCommitmentsQueryRequest, GetPersonalCommitmentsQueryResponse>
 {
     public IAppDbContext _context { get; set; }
     public GetPersonalCommitmentsQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetPersonalCommitmentsQueryResponse> Handle(GetPersonalCommitmentsQueryRequest request, CancellationToken cancellationToken)
         => new GetPersonalCommitmentsQueryResponse()
         {
             Commitments = await _context.Commitments
             .Include(x => x.Behaviour)
             .Include("Behaviour.BehaviourType")
             .Include(x => x.CommitmentFrequencies)
             .Include("CommitmentFrequencies.Frequency")
             .Include("CommitmentFrequencies.Frequency.FrequencyType")
             .Where(x => x.ProfileId == request.ProfileId)
             .Select(x => CommitmentApiModel.FromCommitment(x)).ToListAsync()
         };
 }
