// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.CommitmentAggregate.Queries;

public class GetPersonalCommitmentsRequest : IRequest<GetPersonalCommitmentsResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetPersonalCommitmentsResponse
{
    public IEnumerable<CommitmentDto> Commitments { get; set; }
}

public class GetPersonalCommitmentsQueryHandler : IRequestHandler<GetPersonalCommitmentsRequest, GetPersonalCommitmentsResponse>
{
    public ICommitmentsDbContext _context { get; set; }
    public GetPersonalCommitmentsQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetPersonalCommitmentsResponse> Handle(GetPersonalCommitmentsRequest request, CancellationToken cancellationToken)
        => new GetPersonalCommitmentsResponse()
        {
            Commitments = await _context.Commitments
            .Include(x => x.Behaviour)
            .Include("Behaviour.BehaviourType")
            .Include(x => x.CommitmentFrequencies)
            .Include("CommitmentFrequencies.Frequency")
            .Include("CommitmentFrequencies.Frequency.FrequencyType")
            .Where(x => x.ProfileId == request.ProfileId)
            .Select(x => CommitmentDto.FromCommitment(x)).ToListAsync()
        };
}