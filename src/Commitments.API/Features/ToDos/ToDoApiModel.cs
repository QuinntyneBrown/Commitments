using Commitments.Core.Entities;
using System;


namespace Commitments.Api.Features.ToDos;

public class ToDoApiModel
{        
    public int ToDoId { get; set; }
    public string Name { get; set; }
    public DateTime DueOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public string Description { get; set; }
    public int ProfileId { get; set; }
    public bool IsCompleted { get { return CompletedOn != null; } }
    public static ToDoApiModel FromToDo(ToDo toDo)
        => new ToDoApiModel
        {
            ToDoId = toDo.ToDoId,
            Name = toDo.Name,
            DueOn = toDo.DueOn,
            CompletedOn = toDo.CompletedOn,
            Description = toDo.Description,
            ProfileId = toDo.ProfileId
        };
}
