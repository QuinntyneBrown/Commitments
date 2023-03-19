// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ToDoService.Core;
using Microsoft.EntityFrameworkCore;
using ToDoService.Core.AggregateModel.ToDoAggregate;

namespace ToDoService.Infrastructure.Data;

public class ToDoServiceDbContext : DbContext, IToDoServiceDbContext
{
    public ToDoServiceDbContext(DbContextOptions<ToDoServiceDbContext> options) : base(options)
    {
    }

    public DbSet<ToDo> ToDos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ToDo");

        base.OnModelCreating(modelBuilder);
    }
}


