// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.NoteAggregate.Queries;

public class GetNoteByIdQueryValidator : AbstractValidator<GetNoteByIdRequest>
{
    public GetNoteByIdQueryValidator()
    {
        RuleFor(request => request.NoteId).NotEqual(default(Guid));
    }
}

public class GetNoteByIdRequest : IRequest<GetNoteByIdResponse>
{
    public Guid NoteId { get; set; }
}

public class GetNoteByIdResponse
{
    public NoteDto Note { get; set; }
}

public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdRequest, GetNoteByIdResponse>
{
    private readonly ICommimentsDbContext _context;

    public GetNoteByIdQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetNoteByIdResponse> Handle(GetNoteByIdRequest request, CancellationToken cancellationToken)
        => new GetNoteByIdResponse()
        {
            Note = NoteDto.FromNote(await _context.Notes.FindAsync(request.NoteId))
        };
}

