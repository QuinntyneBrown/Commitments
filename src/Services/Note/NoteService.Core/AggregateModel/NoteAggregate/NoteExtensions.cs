// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.TagAggregate;

namespace NoteService.Core.AggregateModel.NoteAggregate;

public static class NoteExtensions
{
    public static NoteDto ToDto(this Note note)
    {        
        return new NoteDto
        {
            NoteId = note.NoteId,
            Title = note.Title,
            Slug = note.Slug,
            Body = note.Body,
            Tags = note.Tags.Select(x => x.ToDto()).ToList()
        };
    }

    public async static Task<List<NoteDto>> ToDtosAsync(this IQueryable<Note> notes,CancellationToken cancellationToken)
    {
        return await notes.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}


