// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.CardAggregate.Queries;

public class GetCardsRequest : IRequest<GetCardsResponse> { }

public class GetCardsResponse
{
    public IEnumerable<CardDto> Cards { get; set; }
}

public class GetCardsQueryHandler : IRequestHandler<GetCardsRequest, GetCardsResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetCardsQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetCardsResponse> Handle(GetCardsRequest request, CancellationToken cancellationToken)
        => new GetCardsResponse()
        {
            Cards = await _context.Cards.Select(x => CardDto.FromCard(x)).ToListAsync()
        };
}

