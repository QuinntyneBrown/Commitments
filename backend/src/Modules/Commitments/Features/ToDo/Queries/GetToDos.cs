using Commitments.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.ToDo;

public class GetToDosRequest : IRequest<GetToDosResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetToDosResponse
{
    public IEnumerable<ToDoDto> ToDos { get; set; } = Array.Empty<ToDoDto>();
}

public class GetToDosQueryHandler : IRequestHandler<GetToDosRequest, GetToDosResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetToDosQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetToDosResponse> Handle(GetToDosRequest request, CancellationToken cancellationToken)
    {
        var rows = await _context.Todos
            .Where(t => t.ProfileId == request.ProfileId)
            .OrderBy(t => t.CompletedOn != null)
            .ThenBy(t => t.DueOn ?? DateTime.MaxValue)
            .Select(t => ToDoDto.FromToDo(t))
            .ToListAsync(cancellationToken);

        return new GetToDosResponse { ToDos = rows };
    }
}
