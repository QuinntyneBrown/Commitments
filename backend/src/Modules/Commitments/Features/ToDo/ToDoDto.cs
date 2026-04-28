using ToDoEntity = Commitments.Domain.ToDoAggregate.ToDo;

namespace Commitments.Features.ToDo;

public class ToDoDto
{
    public Guid ToDoId { get; set; }
    public Guid ProfileId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueOn { get; set; }
    public DateTime? CompletedOn { get; set; }

    public static ToDoDto FromToDo(ToDoEntity todo) => new()
    {
        ToDoId = todo.ToDoId,
        ProfileId = todo.ProfileId,
        Title = todo.Title,
        Description = todo.Description,
        DueOn = todo.DueOn,
        CompletedOn = todo.CompletedOn
    };
}
