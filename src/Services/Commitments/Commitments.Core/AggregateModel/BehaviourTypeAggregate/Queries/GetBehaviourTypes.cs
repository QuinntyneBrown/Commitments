// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.BehaviourTypeAggregate.Queries;

public class GetBehaviourTypesRequest : IRequest<GetBehaviourTypesResponse> { }

public class GetBehaviourTypesResponse
{
    public IEnumerable<BehaviourTypeDto> BehaviourTypes { get; set; }
}

public class GetBehaviourTypesQueryHandler : IRequestHandler<GetBehaviourTypesRequest, GetBehaviourTypesResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetBehaviourTypesQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetBehaviourTypesResponse> Handle(GetBehaviourTypesRequest request, CancellationToken cancellationToken)
        => new GetBehaviourTypesResponse()
        {
            BehaviourTypes = await _context.BehaviourTypes.Select(x => BehaviourTypeDto.FromBehaviourType(x)).ToListAsync()
        };
}

