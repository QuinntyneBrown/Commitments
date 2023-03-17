// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using System;


namespace Commitments.Api.Features.ToDos;

public class ToDoDto
{        
    public int ToDoId { get; set; }
    public string Name { get; set; }
    public DateTime DueOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public string Description { get; set; }
    public int ProfileId { get; set; }
    public bool IsCompleted { get { return CompletedOn != null; } }
    public static ToDoDto FromToDo(ToDo toDo)
        => new ToDoDto
        {
            ToDoId = toDo.ToDoId,
            Name = toDo.Name,
            DueOn = toDo.DueOn,
            CompletedOn = toDo.CompletedOn,
            Description = toDo.Description,
            ProfileId = toDo.ProfileId
        };
}

