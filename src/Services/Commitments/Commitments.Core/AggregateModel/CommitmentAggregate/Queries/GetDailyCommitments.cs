// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.CommitmentAggregate.Queries;

public class GetDailyCommitmentsRequest : IRequest<GetDailyCommitmentsResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetDailyCommitmentsResponse
{
    public IEnumerable<CommitmentDto> Commitments { get; set; }
}

public class GetDailyCommitmentsQueryHandler : IRequestHandler<GetDailyCommitmentsRequest, GetDailyCommitmentsResponse>
{
    public ICommitmentsDbContext _context { get; set; }
    public GetDailyCommitmentsQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetDailyCommitmentsResponse> Handle(GetDailyCommitmentsRequest request, CancellationToken cancellationToken)
        => new GetDailyCommitmentsResponse()
        {
            Commitments = await _context.Commitments
            .Include(x => x.Behaviour)
            .Include("Behaviour.BehaviourType")
            .Include(x => x.CommitmentFrequencies)
            .Include("CommitmentFrequencies.Frequency")
            .Include("CommitmentFrequencies.Frequency.FrequencyType")
            .Where(x => x.ProfileId == request.ProfileId && x.CommitmentFrequencies.Any(f => f.Frequency.FrequencyType.Name == "per day"))
            .Select(x => CommitmentDto.FromCommitment(x)).ToListAsync()
        };
}