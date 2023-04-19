// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.FrequencyTypeAggregate.Queries;

public class GetFrequencyTypesRequest : IRequest<GetFrequencyTypesResponse> { }

public class GetFrequencyTypesResponse
{
    public IEnumerable<FrequencyTypeDto> FrequencyTypes { get; set; }
}

public class GetFrequencyTypesQueryHandler : IRequestHandler<GetFrequencyTypesRequest, GetFrequencyTypesResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public GetFrequencyTypesQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetFrequencyTypesResponse> Handle(GetFrequencyTypesRequest request, CancellationToken cancellationToken)
        => new GetFrequencyTypesResponse()
        {
            FrequencyTypes = await _context.FrequencyTypes.Select(x => FrequencyTypeDto.FromFrequencyType(x)).ToListAsync()
        };
}