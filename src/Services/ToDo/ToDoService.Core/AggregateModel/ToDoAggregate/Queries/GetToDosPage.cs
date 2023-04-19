// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate.Queries;

public class GetToDosPageRequest : IRequest<GetToDosPageResponse>
{
    public int PageSize { get; set; }
    public int Index { get; set; }
    public int Length { get; set; }
}


public class GetToDosPageResponse
{
    public required int Length { get; set; }
    public required List<ToDoDto> Entities { get; set; }
}


public class CreateToDoRequestHandler : IRequestHandler<GetToDosPageRequest, GetToDosPageResponse>
{
    private readonly IToDoServiceDbContext _context;

    private readonly ILogger<CreateToDoRequestHandler> _logger;

    public CreateToDoRequestHandler(ILogger<CreateToDoRequestHandler> logger, IToDoServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetToDosPageResponse> Handle(GetToDosPageRequest request, CancellationToken cancellationToken)
    {
        var query = from toDo in _context.ToDos
                    select toDo;

        var length = await _context.ToDos.AsNoTracking().CountAsync();

        var toDos = await query.Page(request.Index, request.PageSize).AsNoTracking()
            .Select(x => x.ToDto()).ToListAsync();

        return new()
        {
            Length = length,
            Entities = toDos
        };

    }

}