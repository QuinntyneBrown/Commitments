// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.ActivityAggregate.Queries;

public class GetActivitiesRequest : IRequest<GetActivitiesResponse> { }

public class GetActivitiesResponse
{
    public IEnumerable<ActivityDto> Activities { get; set; }
}

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesRequest, GetActivitiesResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public GetActivitiesQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetActivitiesResponse> Handle(GetActivitiesRequest request, CancellationToken cancellationToken)
        => new GetActivitiesResponse()
        {
            Activities = await _context.Activities
            .Include(x => x.Behaviour)
            .Include("Behaviour.BehaviourType")
            .Select(x => ActivityDto.FromActivity(x)).ToListAsync()
        };
}