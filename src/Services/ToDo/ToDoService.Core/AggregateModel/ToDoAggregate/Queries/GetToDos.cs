// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate.Queries;

public class GetToDosRequest : IRequest<GetToDosResponse> { }

public class GetToDosResponse
{
    public required List<ToDoDto> ToDos { get; set; }
}


public class GetToDosRequestHandler : IRequestHandler<GetToDosRequest, GetToDosResponse>
{
    private readonly IToDoServiceDbContext _context;

    private readonly ILogger<GetToDosRequestHandler> _logger;

    public GetToDosRequestHandler(ILogger<GetToDosRequestHandler> logger, IToDoServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetToDosResponse> Handle(GetToDosRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            ToDos = await _context.ToDos.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}