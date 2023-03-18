// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ToDoService.Core.AggregateModel.ToDoAggregate;

namespace ToDoService.Core;

public interface IToDoServiceDbContext
{
    DbSet<ToDo> ToDos { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}