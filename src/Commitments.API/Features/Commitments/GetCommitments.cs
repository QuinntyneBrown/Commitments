// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Commitments;

 public class GetCommitmentsRequest : IRequest<GetCommitmentsResponse> { }

 public class GetCommitmentsResponse
 {
     public IEnumerable<CommitmentDto> Commitments { get; set; }
 }

 public class GetCommitmentsQueryHandler : IRequestHandler<GetCommitmentsRequest, GetCommitmentsResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetCommitmentsQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetCommitmentsResponse> Handle(GetCommitmentsRequest request, CancellationToken cancellationToken)
         => new GetCommitmentsResponse()
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

