// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.ToDoAggregate;
using System;


namespace Commitments.Core.AggregateModel.ToDoAggregate;

public class ToDoDto
{
    public Guid ToDoId { get; set; }
    public string Name { get; set; }
    public DateTime DueOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public string Description { get; set; }
    public Guid ProfileId { get; set; }
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

