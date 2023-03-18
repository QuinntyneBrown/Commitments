// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.BehaviourAggregate.Queries;

public class GetBehavioursRequest : IRequest<GetBehavioursResponse> { }

public class GetBehavioursResponse
{
    public IEnumerable<BehaviourDto> Behaviours { get; set; }
}

public class GetBehavioursQueryHandler : IRequestHandler<GetBehavioursRequest, GetBehavioursResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetBehavioursQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetBehavioursResponse> Handle(GetBehavioursRequest request, CancellationToken cancellationToken)
        => new GetBehavioursResponse()
        {
            Behaviours = await _context.Behaviours.Include(x => x.BehaviourType).Select(x => BehaviourDto.FromBehaviour(x)).ToListAsync()
        };
}

