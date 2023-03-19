// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core;
using Microsoft.EntityFrameworkCore;
using NoteService.Core.AggregateModel.NoteAggregate;
using NoteService.Core.AggregateModel.TagAggregate;

namespace NoteService.Infrastructure.Data;

public class NoteServiceDbContext : DbContext, INoteServiceDbContext
{
    public NoteServiceDbContext(DbContextOptions<NoteServiceDbContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Note");

        base.OnModelCreating(modelBuilder);
    }
}


