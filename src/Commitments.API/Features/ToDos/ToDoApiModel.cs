using Commitments.Core.Entities;
using System;

namespace Commitments.API.Features.ToDos
{
    public class ToDoApiModel
    {        
        public int ToDoId { get; set; }
        public string Name { get; set; }
        public DateTime DueOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string Description { get; set; }
        public int ProfileId { get; set; }
        public static ToDoApiModel FromToDo(ToDo toDo)
        {
            var model = new ToDoApiModel();
            model.ToDoId = toDo.ToDoId;
            model.Name = toDo.Name;
            model.DueOn = toDo.DueOn;
            model.CompletedOn = toDo.CompletedOn;
            model.Description = toDo.Description;
            model.ProfileId = toDo.ProfileId;
            return model;
        }
    }
}
