using Commitments.Data;
using FluentValidation;
using MediatR;
using ToDoEntity = Commitments.Domain.ToDoAggregate.ToDo;

namespace Commitments.Features.ToDo;

public class SaveToDoCommandValidator : AbstractValidator<SaveToDoRequest>
{
    public SaveToDoCommandValidator()
    {
        RuleFor(r => r.ToDo.Title).NotEmpty();
    }
}

public class SaveToDoRequest : IRequest<SaveToDoResponse>
{
    public ToDoDto ToDo { get; set; } = new();
}

public class SaveToDoResponse
{
    public Guid ToDoId { get; set; }
}

public class SaveToDoCommandHandler : IRequestHandler<SaveToDoRequest, SaveToDoResponse>
{
    private readonly ICommitmentsDbContext _context;

    public SaveToDoCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<SaveToDoResponse> Handle(SaveToDoRequest request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.FindAsync(new object?[] { request.ToDo.ToDoId }, cancellationToken);
        if (todo is null) _context.Todos.Add(todo = new ToDoEntity { ProfileId = request.ToDo.ProfileId });

        todo.Title = request.ToDo.Title;
        todo.Description = request.ToDo.Description;
        todo.DueOn = request.ToDo.DueOn;

        await _context.SaveChangesAsync(cancellationToken);
        return new SaveToDoResponse { ToDoId = todo.ToDoId };
    }
}
