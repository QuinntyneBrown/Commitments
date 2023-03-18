// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate;

public static class ToDoExtensions
{
    public static ToDoDto ToDto(this ToDo toDo)
    {
        return new ToDoDto
        {
            ToDoId = toDo.ToDoId,
            Name = toDo.Name,
            Description = toDo.Description,
            ProfileId = toDo.ProfileId,
            Due = toDo.Due,
            CompletedOn = toDo.CompletedOn,
        };

    }

    public async static Task<List<ToDoDto>> ToDtosAsync(this IQueryable<ToDo> toDos,CancellationToken cancellationToken)
    {
        return await toDos.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}


