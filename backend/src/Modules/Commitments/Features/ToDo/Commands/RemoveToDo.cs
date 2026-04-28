using Commitments.Data;
using MediatR;

namespace Commitments.Features.ToDo;

public class RemoveToDoRequest : IRequest
{
    public Guid ToDoId { get; set; }
}

public class RemoveToDoCommandHandler : IRequestHandler<RemoveToDoRequest>
{
    private readonly ICommitmentsDbContext _context;

    public RemoveToDoCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveToDoRequest request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.FindAsync(new object?[] { request.ToDoId }, cancellationToken);
        if (todo is null) return;

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
