// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core;
using Microsoft.EntityFrameworkCore;
using NoteService.Core.AggregateModel.NoteAggregate;
using NoteService.Core.AggregateModel.TagAggregate;

namespace NoteService.Core;

public interface INoteServiceDbContext
{
    DbSet<Note> Notes { get; set; }
    DbSet<Tag> Tags { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}


