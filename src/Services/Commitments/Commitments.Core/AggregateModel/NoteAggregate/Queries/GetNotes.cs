// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.NoteAggregate.Queries;

public class GetNotesRequest : IRequest<GetNotesResponse> { }

public class GetNotesResponse
{
    public IEnumerable<NoteDto> Notes { get; set; }
}

public class GetNotesQueryHandler : IRequestHandler<GetNotesRequest, GetNotesResponse>
{
    private readonly ICommimentsDbContext _context;

    public GetNotesQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetNotesResponse> Handle(GetNotesRequest request, CancellationToken cancellationToken)
        => new GetNotesResponse()
        {
            Notes = await _context.Notes.Select(x => NoteDto.FromNote(x, true)).ToListAsync()
        };
}

