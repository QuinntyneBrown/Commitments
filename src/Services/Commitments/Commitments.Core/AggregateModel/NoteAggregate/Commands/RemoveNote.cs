// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Core.AggregateModel.NoteAggregate.Commands;

public class RemoveNoteCommandValidator : AbstractValidator<RemoveNoteRequest>
{
    public RemoveNoteCommandValidator()
    {
        RuleFor(request => request.NoteId).NotEqual(default(Guid));
    }
}

public class RemoveNoteRequest : IRequest<RemoveNoteResponse>
{
    public Guid NoteId { get; set; }
}

public class RemoveNoteResponse { }

public class RemoveNoteCommandHandler : IRequestHandler<RemoveNoteRequest, RemoveNoteResponse>
{
    private readonly ICommimentsDbContext _context;

    public RemoveNoteCommandHandler(ICommimentsDbContext context) => _context = context;

    public async Task<RemoveNoteResponse> Handle(RemoveNoteRequest request, CancellationToken cancellationToken)
    {
        _context.Notes.Remove(await _context.Notes.FindAsync(request.NoteId));
        await _context.SaveChangesAsync(cancellationToken);
        return new RemoveNoteResponse() { };
    }
}

