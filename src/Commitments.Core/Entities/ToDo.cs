using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Commitments.Core.Entities;

public class ToDo: BaseEntity
{
    public int ToDoId { get; set; }           
    public string Description { get; set; }
    [ForeignKey("Profile")]
    public int ProfileId { get; set; }
    public DateTime DueOn { get; set; }
    public DateTime? CompletedOn { get; set; }
    public Profile Profile { get; set; }
}
