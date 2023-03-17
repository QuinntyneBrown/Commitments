using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Commitments;

 public class GetCommitmentsQueryRequest : IRequest<GetCommitmentsQueryResponse> { }

 public class GetCommitmentsQueryResponse
 {
     public IEnumerable<CommitmentDto> Commitments { get; set; }
 }

 public class GetCommitmentsQueryHandler : IRequestHandler<GetCommitmentsQueryRequest, GetCommitmentsQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetCommitmentsQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetCommitmentsQueryResponse> Handle(GetCommitmentsQueryRequest request, CancellationToken cancellationToken)
         => new GetCommitmentsQueryResponse()
         {
             Commitments = await _context.Commitments
             .Include(x => x.Behaviour)
             .Include("Behaviour.BehaviourType")
             .Include(x => x.CommitmentFrequencies)
             .Include("CommitmentFrequencies.Frequency")
             .Include("CommitmentFrequencies.Frequency.FrequencyType")
             .Select(x => CommitmentDto.FromCommitment(x)).ToListAsync()
         };
 }
