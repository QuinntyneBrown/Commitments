using Commitments.Data;
using MediatR;

namespace Commitments.Features.ToDo;

public class CompleteToDoRequest : IRequest
{
    public Guid ToDoId { get; set; }
}

public class CompleteToDoCommandHandler : IRequestHandler<CompleteToDoRequest>
{
    private readonly ICommitmentsDbContext _context;

    public CompleteToDoCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(CompleteToDoRequest request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.FindAsync(new object?[] { request.ToDoId }, cancellationToken);
        if (todo is null || todo.CompletedOn is not null) return;

        todo.CompletedOn = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
